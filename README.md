강력한 게임 데이트 관리 툴
==========================

2월 23일 정식오픈입니다.

카카오톡 오픈 채팅방 주소 https://open.kakao.com/o/gu27H4G

개념
-------------

엑셀 기반의 정적 데이터 변환과 적용을 자동화시킨 툴이며

기획자의 원터치 벨런스 수정을 위해 제작되었습니다.

설명
-------------

엑셀 문서를 기반으로 클라이언트와 서버에서 사용하는 바이너리 파일을 추출하고 해당 코드를 자동으로 작성는, 벨런스 수정을 위한 후반 작업을 자동화 시킨 툴입니다.


즉 엑셀에서 벨런스를 수정하고 Ctrl + r을 누르면 클라이언트와 서버 프로그래머의 도움없이 자동으로 바이너리파일을 생성하고 해당소스 코드가 작성되어, 프로그래머의 도움이나 별다른 조작없이 게임 클라이언트를 실행하여 조정된 벨런스를 테스트할 수 있습니다.

장점
-------------

1. 프로젝트 초반 데이터 변환 관련 작업이 필요없습니다.
	- MarkTwo가 자동으로 엑셀에서 데이터를 추출하며 해당 데이타를 기반으로 코드를 작성하기 때문에 프로젝트 초반에 엑셀 데이터를 추출하기 위한 코드 작업이 필요 없습니다.

2. 벨런스 변화에 따른 프로그래머의 작업 요청이 발생하지 않습니다.
	- 원터치로 변환된 벨런스를 적용하기 때문에 프로그램 파트와 독립적으로 벨런스를 수정 및 테스트, 배포가 가능합니다.
	- ※ 단 필드 구성 변화는 게임 동작 관련 변화이기 때문에 변환 후 프로그래머와 상의하셔야 합니다.

3. 자동 백업에 따른 버전관리
	- 빌드 시 자동으로 엑셀 파일을 백업하기 때문에 버전관리가 용이합니다.


주요기능
-------------
1. 필드 수정 시 클라이언트 및 서버 자동 업데이트
2. 레코드 추가 시 클라이언트 및 서버 자동 업데이트
3. 엑셀 필드에 주석을 넣을 수 있어야 하며, 클라이언트와 서버는 이를 판별해야 한다.
4. 엑셀 레이블에 주석을 넣을 수 있어야 하며, 클라이언트와 서버는 이를 판별해야 한다.
5. 엑셀 시트는 클라이언트 독립적 혹은 서버 독립적 및 동시 사용에 대한 구분이 있어야 한다.
6. 5의 기능에 따라 클라이언트와 서버는 자동 업데이트를 해야 한다.
7. 엑셀 필드는 서버 및 클라이언트 독립적인 사용이 가능해야 하며,
8. 7의 기능에 따라 클라이언트와 서버는 자동 업데이트를  해야 한다.
9. 잘못된 자료형이나 데이터의 입력이 감지되어야 한다.
10. 9에 해당하는 데이터의 위치가 어디인지 표시되어야 한다.
11. 생성된 파일은 자동으로 해당 폴더에 배치되어야 한다.
12. 바이너리 파일은 클라이언트 및 서버 실행 시 자동으로 읽어 들여야 한다.
13. 엑셀에서 수정 후 테스트까지(기능 구현 및 수정에 따른 부분은 제외) 완전 자동화가 이루어져야 한다.

동작
-------------
1. 클라이언트 바이너리 파일 생성
2. 클라이언트 파싱클래스(바이너리를 로드하는) 파일 생성
3. 클라이언트 테이블 클래스(엑셀 테이블에 대응하는) 파일 생성
4. 클라라이언트 Tag 리스트 생성
5. 사용자 정의 enum 인식 및 적용
6. 레디스 서버 접속 및 Db1 초기화(서버용 엑셀 데이트가 들어있는)
7. 레디스 서버에 해당 엑셀의 서버용(클라이언트 전용은 필터링됨) 데이터 전송
8. 엑셀 테이블 버전 업데이트
9. 엑셀 백업
10. 생성된 파일 자동 배치
11. 클라이언트 바이너리 파일 다운로드 폴더로 이동
12. 서버 자동 세팅


부가 기능
-------------
1. SQLite에 데이터 전송
2. MySQL에 데이터 전송
3. 서버용 바이너리 파일 생성
4. PHP 테이블 클래스 생성


라이센스 
-------------
The MIT License - Copyright (c) 2018 SeongKyuKo


모두 건승하시기 바랍니다 :)
============================
