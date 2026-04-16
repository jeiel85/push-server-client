## 🔄 v1.1.0 - UI 개선

### ✨ 새로운 기능

- **애플리케이션 스타일 UI** - 전문적인 프로그램 느낌
- **다크 테마** - 배경 #2D2D30, 텍스트 #DCDCDC
- **툴바** - 연결/연결해제/전송/초기화 버튼
- **상태바** - 연결 상태 LED, 세션ID, 실시간 시계
- **탭 컨트롤** - Request/Response 분리
- **RichTextBox** - Consolas 폰트, JSON 포맷팅 지원
- **연결 상태 콜백** - OnOpen/OnClose 이벤트 핸들링
- **로그 타임스탬프** - 각 로그에 [HH:mm:ss] prefix

### 📝 빌드 방법

```
Visual Studio에서 Poscle35.sln 열기
빌드 > 솔루션 빌드 (Ctrl+Shift+B)
Poscle35/bin/Debug/Poscle35.exe 실행
```

### 📄 문서

- `README.md` - 프로젝트 설명 및 사용법
- `PROJECT_STRUCTURE.md` - 상세 아키텍처 문서

### 🔧 기술 스택

- 서버: .NET Core 3.1 + SuperSocket
- 클라이언트: .NET Framework 3.5 + Windows Forms
