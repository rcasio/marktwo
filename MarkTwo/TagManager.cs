﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkTwo
{
    public static class TagManager
    {
        // 시트를 구분하기 위해서 사용된다.
        public enum SheetType { None ,Multilingual, Client, Server }
        
        // 시트 이름
        public class SheetName
        {
            public const string Multilingual = "Multilingual"; // 다국어 시트
            public const string PR = "PR"; // PR 시트
            public const string Tag = "Tag"; // Tag 시트
        }

        // 파일 확장자
        public class FileExtensionName
        {
            public const string Binary = "bytes";
        }
    }
}
