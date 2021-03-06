﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Reflection.Emit;

using static MarkTwo.TagManager;

namespace MarkTwo
{
    public class DataType
    {
        Excel.Worksheet ruleSheet; // [테이블_규칙] 시트
        Excel.Worksheet tagSheet; // [Tag] 시트
        DataManager dataManager;
        DataRule dataRule;

        const int SUPPROT_TYPE_COUNT = 8; // 클라이언트의 자료형 개수를 나타낸다. 만약 자료형이 추가 및 삭제된다면 이부분을 수정한다.

        public Dictionary<string, Type> cSharpTypes = new Dictionary<string, Type>(); // c# 자료형
        public Dictionary<string, Type> mySQLTypes = new Dictionary<string, Type>(); // MySQL 자료형
        
        public DataType(Excel.Worksheet ruleSheet, 
                        Excel.Worksheet tagSheet,
                        DataManager dataManager, 
                        DataRule dataRule, 
                        Action<int> SetExtreactionProgressBar, 
                        Action<RichTextBox, string> SetRichText)
        {
            RichTextBox rb = dataManager.converterWindow.ExtreactionReadyText; // 데이터 추출 준비 리치텍스트 박스 폼을 참조한다.

            SetRichText(rb, "====== 테이블 규칙 설정 \n[테이블_규칙], [Tag] 시트에서 데이터 타입 설정을 시작합니다.");

            this.ruleSheet = ruleSheet;
            this.dataManager = dataManager;
            this.tagSheet = tagSheet;
            this.dataRule = dataRule;

            // 8개의 자료형을 가진다. 만약 엑셀에서 자료형을 추가한다면 이 부분을 수정해야 한다.
            for (int i = 0; i < SUPPROT_TYPE_COUNT; i++)
            {
                string range = null;
                string typeText = null;

                // C# 자료형 추출
                range = "D" + (22 + i).ToString();
                typeText = ruleSheet.Range[range].Value;

                cSharpTypes.Add(typeText, this.GetCShapType(typeText)); // 리스트에 자료형을 등록한다.
                
                // MYSQL 자료형 추출
                range = "C" + (22 + i).ToString();
                typeText = ruleSheet.Range[range].Value;

                mySQLTypes.Add(typeText, this.GetMySQLType(typeText));
                //ClientTypeList.Text += type + "\n"; // 라벨에 표시한다.
            }

            SetExtreactionProgressBar(20);

            SheetData tagSheetData = new SheetData(dataManager.sheets[SheetName.Tag] as Excel.Worksheet, 
                                                   SheetName.Tag, 
                                                   this.dataManager, 
                                                   this.dataRule, 
                                                   SetRichText, 
                                                   this.dataManager.converterWindow.ExtreactionReadyText,
                                                   false,
                                                   SheetType.None); // 태그 시트 정보를 추출한다.

            tagSheetData.Create(SetRichText, this.dataManager.converterWindow.ExtreactionReadyText);

            // enum 타입 추가
            foreach (var key in tagSheetData.fieldDatas.Keys)
            {
                FieldData fieldData = tagSheetData.fieldDatas[key]; // 필드 데이터(enum 자료형)를 추출한다.

                if (!fieldData.name.Equals("Num")) // Num 필드는 enum을 생성하지 않는다.
                {
                    List<string> net_List = fieldData.contents; // enum의 멤버를 추가한다.

                    Type netListEnumType = this.GenerateEnumerations(net_List, key); // enum 을 생성한다.
                    
                    SetRichText(rb, "");
                    SetRichText(rb, "동적 생성 Enum 이름: " + netListEnumType.Name);
                    SetRichText(rb, "- 실제 이름 : " + netListEnumType.GetType().Name);

                    foreach (var item in net_List)
                    {
                        if (string.IsNullOrEmpty(item)) break;

                        var enumValBoxed = Enum.Parse(netListEnumType, item);
                        SetRichText(rb, "- 멤버 : " + enumValBoxed.ToString());
                    }
                    
                    cSharpTypes.Add(netListEnumType.Name, netListEnumType);
                    mySQLTypes.Add(netListEnumType.Name, netListEnumType);
                }
            }

            SetExtreactionProgressBar(30);
            
            SetRichText(rb, "");
            SetRichText(rb, "엑셀 지원 자료형");
            SetRichText(rb, "");
            SetRichText(rb, "c# 자료형");

            foreach (var cSharpType in cSharpTypes.Keys)
            {
                SetRichText(rb, "- 엑셀 스트링 : " + cSharpType + " => 시스템 자료형 : " + cSharpTypes[cSharpType]);
            }
            
            SetRichText(rb, "");
            SetRichText(rb, "MySQL 자료형");
            foreach (var mySQType in mySQLTypes.Keys)
            {
                SetRichText(rb, "- 엑셀 스트링 : " + mySQType + " => 시스템 자료형 : " + mySQLTypes[mySQType]);
            }

            SetExtreactionProgressBar(40);
            SetRichText(rb, "====== 완료");
        }

        /// <summary>
        /// MySQL 자료형을 추출한다.
        /// </summary>
        /// <param name="text">엑셀에 기록되어 있는 MySQL 타입</param>
        /// <returns>MySQL Type</returns>
        private Type GetMySQLType(string text)
        {
            Type type = null;

            if (text.Equals("Bit")) type = typeof(bool);
            if (text.Equals("TinyInt")) type = typeof(byte);
            if (text.Equals("SmallInt")) type = typeof(short);
            if (text.Equals("Int")) type = typeof(int);
            if (text.Equals("Float")) type = typeof(float);
            if (text.Equals("Double")) type = typeof(double);
            if (text.Equals("Bigint")) type = typeof(long);
            if (text.Equals("VarChar")) type = typeof(string);

            return type;
        }

        /// <summary>
        /// c# 타입을 리턴한다.
        /// </summary>
        /// <param name="text"> 엑셀에 기록되어 있는 c# 타입</param>
        /// <returns>C# type</returns>
        public Type GetCShapType(string text)
        {
            Type type = null;

            if (text.Equals("bool")) type = typeof(bool);
            if (text.Equals("byte")) type = typeof(byte);
            if (text.Equals("short")) type = typeof(short);
            if (text.Equals("int")) type = typeof(int);
            if (text.Equals("float")) type = typeof(float);
            if (text.Equals("double")) type = typeof(double);
            if (text.Equals("long")) type = typeof(long);
            if (text.Equals("string")) type = typeof(string);

            return type;
        }

        /// <summary>
        /// enum을 동적으로 생성한다.
        /// </summary>
        /// <param name="lEnumItems"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public Type GenerateEnumerations(List<string> lEnumItems, string assemblyName)
        {
            AppDomain appDomain = AppDomain.CurrentDomain;
            AssemblyName asmName = new AssemblyName(assemblyName);
            AssemblyBuilder asmBuilder = appDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);

            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(assemblyName + "_module");
            EnumBuilder enumBuilder = modBuilder.DefineEnum(assemblyName, TypeAttributes.Public, typeof(int));
            enumBuilder.DefineLiteral("None", 0);

            int flagCnt = 1;

            foreach (string fmtObj in lEnumItems)
            {
                if (string.IsNullOrEmpty(fmtObj)) break;

                enumBuilder.DefineLiteral(fmtObj, flagCnt);
                flagCnt++;
            }

            var retEnumType = enumBuilder.CreateType();

            return retEnumType;
        }

        public bool CheckMySQLType(string data) // 타입을 체크한다
        {
            if (this.mySQLTypes.ContainsKey(data) ||
                data.StartsWith("VarChar("))
            {
                return true;
            }
            else return false;
        }
    }
}
