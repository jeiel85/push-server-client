# 프로젝트 구조 문서 (PROJECT_STRUCTURE.md)

> **프로젝트명**: push-server-client
> **내용**: WebSocket 기반 실시간 푸시 알림 시스템 (서버 + 클라이언트 샘플)
> **마지막 업데이트**: 2026-04-16

---

## 📁 프로젝트 전체 구조

```
push-server-client/
│
├── 📂 ws002/                          # 🔷 WebSocket 서버 프로젝트 (.NET Core 3.1)
│   ├── 📄 Program.cs                   # 서버 엔트리포인트
│   ├── 📄 StringPackageConverter.cs   # 패킷 변환기 (JSON 파싱)
│   ├── 📄 appsettings.json             # 서버 설정 파일
│   │
│   ├── 📂 Commands/                    # 명령 핸들러 디렉토리
│   │   ├── 📄 noneProc.cs             # 기본 처리 명령
│   │   ├── 📄 connectList.cs          # 연결 목록 조회 명령
│   │   ├── 📄 MSG.cs                  # 메시지 전송 명령
│   │   └── 📄 COMMAND_CON.cs          # 클라이언트 연결 처리 명령
│   │
│   ├── 📂 Services/                    # 서비스 레이어 디렉토리
│   │   ├── 📄 UserSession.cs          # WebSocket 세션 관리
│   │   └── 📄 UserService.cs          # 사용자/연결 관리 서비스
│   │
│   └── 📂 Models/                      # 데이터 모델 디렉토리
│       ├── 📄 MsgBin.cs               # 메시지 데이터 모델
│       └── 📄 LogBin.cs               # 로그/연결 데이터 모델
│
├── 📂 Poscle35/                        # 🟢 Windows Forms 클라이언트 (.NET Framework 3.5)
│   ├── 📄 Program.cs                   # WinForms 엔트리포인트
│   ├── 📄 Form1.cs                    # 메인 폼 코드
│   ├── 📄 Form1.Designer.cs           # WinForms 디자이너 코드
│   ├── 📄 FormSub.cs                  # 폼 헬퍼 메서드 (스레드 세이프 로그)
│   ├── 📄 WScle.cs                    # WebSocket 클라이언트 래퍼
│   │
│   ├── 📂 Models/                     # 데이터 모델 디렉토리
│   │   ├── 📄 TokenModel.cs          # 토큰 모델
│   │   ├── 📄 SessoinModel.cs        # 세션 모델
│   │   ├── 📄 CommandBin.cs          # 명령 데이터 모델
│   │   ├── 📄 MsgBin.cs              # 메시지 데이터 모델
│   │   └── 📄 LogBin.cs              # 로그 데이터 모델
│   │
│   └── 📂 Properties/                  # 프로젝트 속성
│       ├── 📄 AssemblyInfo.cs
│       ├── 📄 Resources.resx / Resources.Designer.cs
│       └── 📄 Settings.settings / Settings.Designer.cs
│
├── 📄 ws002.sln                       # Visual Studio 솔루션 파일
├── 📄 README.md                        # 프로젝트 기본 설명
├── 📄 .gitignore                       # Git 무시 파일
└── 📄 .gitattributes                   # Git 속성 파일
```

---

## 🏗️ 아키텍처 개요

### 시스템 구성도

```
┌─────────────────────────────────────────────────────────────────┐
│                        Client (Poscle35)                         │
│                   .NET Framework 3.5 WinForms                    │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────────┐  │
│  │  Menu List  │  │   Panel     │  │    Request/Response     │  │
│  │  (commands) │  │  (controls) │  │       TextBoxes         │  │
│  └─────────────┘  └─────────────┘  └─────────────────────────┘  │
│                            │                                     │
│                     ┌──────┴──────┐                              │
│                     │  WScle.cs   │                              │
│                     │ (WebSocket  │                              │
│                     │   Client)   │                              │
│                     └──────┬──────┘                              │
└────────────────────────────┼────────────────────────────────────┘
                             │ websocket-sharp
                             │ ws://localhost:7000/
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                       Server (ws002)                              │
│                      .NET Core 3.1                               │
│  ┌─────────────────────────────────────────────────────────────┐ │
│  │              SuperSocket.WebSocket.Server                    │ │
│  │                        (포트 7000)                           │ │
│  └─────────────────────────────────────────────────────────────┘ │
│                            │                                     │
│         ┌──────────────────┼──────────────────┐                  │
│         ▼                  ▼                  ▼                  │
│  ┌─────────────┐   ┌─────────────┐   ┌─────────────────┐        │
│  │  Commands   │   │  Services   │   │    Models       │        │
│  │             │   │             │   │                 │        │
│  │ • noneProc  │   │UserSession  │   │ • MsgBin        │        │
│  │ • connectList   │UserService  │   │ • LogBin        │        │
│  │ • CON       │   │             │   │                 │        │
│  │ • MSG       │   │             │   │                 │        │
│  └─────────────┘   └─────────────┘   └─────────────────┘        │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔷 서버 프로젝트 (ws002) 상세

### 기술 스택

| 항목 | 내용 |
|------|------|
| **프레임워크** | .NET Core 3.1 |
| **WebSocket** | SuperSocket.WebSocket.Server v2.0.0-beta.8 |
| **JSON** | Newtonsoft.Json v13.0.3 |
| **포트** | 7000 (설정 가능) |
| **프로토콜** | WebSocket (JSON 기반 커스텀 명령) |

### 엔트리포인트 (Program.cs)

```csharp
// 주요 구성 요소:
// 1. WebSocketHostBuilder.Create() - WebSocket 서버 생성
// 2. UseSession<UserSession>() - 커스텀 세션 사용
// 3. UseCommand<StringPackageInfo, StringPackageConverter>() - 명령 기반 처리
// 4. ConfigureServices() - DI 컨테이너 구성
```

### 명령 시스템 (Commands/)

| 명령 | 파일 | 기능 |
|------|------|------|
| `noneProc` | noneProc.cs | 기본 처리 (로깅만) |
| `connectList` | connectList.cs | 현재 연결된 클라이언트 목록 조회 |
| `CON` | COMMAND_CON.cs | 클라이언트 연결 처리 + 세션 등록 |
| `MSG` | MSG.cs | 특정 클라이언트로 메시지 전송 |

### 패킷 변환 (StringPackageConverter.cs)

```
수신 패킷 형식:
{
  "Key": "COMMAND_NAME",
  "Body": { ... }
}

변환 후:
- Key: 명령 이름 추출
- Body: Body 객체만 직렬화
```

### 세션 관리 (Services/)

**UserSession.cs** - WebSocket 세션 속성:
- `deviceid`: 디바이스 식별자
- `com_id`: 회사/가맹점 ID
- `rct_code`: 수신자 코드

**UserService.cs** - 핵심 기능:
- `EnterRoom()`: 세션 등록
- `LeaveRoom()`: 세션 해제
- `getConnectList()`: 연결 목록 조회
- `getToSession()`: 특정 세션 조회 (com_id + rct_code)
- `BroadcastMessage()`: 전체 브로드캐스트

### 데이터 모델 (Models/)

**MsgBin.cs** - 메시지 전송:
```csharp
{
  com_id: "wavepostest",      // 가맹점 ID
  rct_code: "1002",          // 수신자 코드
  order_info: "{...}"        // 주문 정보 JSON
}
```

**LogBin.cs** - 연결 로그:
```csharp
{
  com_id: "wavepostest",      // 가맹점 ID
  rct_code: "1002",          // 수신자 코드
  deviceId: "123456"         // 디바이스 ID
}
```

---

## 🟢 클라이언트 프로젝트 (Poscle35) 상세

### 기술 스택

| 항목 | 내용 |
|------|------|
| **프레임워크** | .NET Framework 3.5 |
| **UI** | Windows Forms |
| **WebSocket** | websocket-sharp v1.0.3-rc11 |
| **JSON** | Newtonsoft.Json v13.0.3 |
| **연결 주소** | ws://localhost:7000/ |

### UI 구조 (Form1.Designer.cs)

```
┌────────────────────────────────────────────────────────────────┐
│ Panel (상단)                                                    │
│ ┌──────────────┬────────────┬──────────┬────────────┬────────┐ │
│ │ ServerSelect │  SessionID │  ClearBtn│  WSsendBtn │ myID  │ │
│ │   ComboBox   │   Label    │  Button  │   Button   │ Label │ │
│ └──────────────┴────────────┴──────────┴────────────┴────────┘ │
├─────────────┬──────────────────────────────────────────────────┤
│             │                                                  │
│  MenuList   │  ReqLog (좌측)    │    ResLog (우측)              │
│  ListBox    │  요청 로그        │    응답 로그                  │
│             │                   │                              │
│ • WS connect│                   │                              │
│ • WS Disc...│                   │                              │
│ • WS 1st con│                   │                              │
│ • WS conn...│                   │                              │
│ • WS to send│                   │                              │
│             │                   │                              │
└─────────────┴───────────────────┴──────────────────────────────┘
```

### WebSocket 클라이언트 (WScle.cs)

**주요 메서드:**
- `connectWS(int idx)`: 서버 연결 (ws://localhost:7000/)
- `closeWS()`: 연결 종료
- `sendWS()`: 메시지 전송
- `reseiveWS(string msg)`: 응답 처리
- `getReqData(string key)`: 요청 데이터 생성

**지원 명령:**
1. **WS 1st con** → CON 명령 → 세션ID 반환
2. **WS connectList** → connectList 명령 → 연결 목록 반환
3. **WS to send** → MSG 명령 → 특정 클라이언트로 주문 정보 전송

### 스레드 세이프 로그 (FormSub.cs)

```csharp
// WinForms에서는 비동기 콜백에서 UI 업데이트 시 Invoke 필요
private void LogWrite(string buf)
{
    if (ResLog.InvokeRequired)
        ResLog.Invoke(new MethodInvoker(delegate { ResLog.Text += ...; }));
    else
        ResLog.Text += buf + Environment.NewLine;
}
```

### 데이터 모델 (Models/)

클라이언트와 서버의 모델이 동일하게 공유됩니다:
- `MsgBin.cs` - 메시지 바인딩
- `LogBin.cs` - 로그 바인딩
- `CommandBin.cs` - 명령 바인딩
- `SessoinModel.cs` - 세션 응답
- `TokenModel.cs` - 토큰 (미사용)

---

## 🔄 통신 흐름

### 1. 연결 및 세션 등록

```
클라이언트                          서버
   │                                │
   │──── WS connect ──────────────▶│ (WebSocket 연결 수립)
   │                                │
   │◀─── "connected success" ──────│
   │                                │
   │──── CON {deviceId, com_id, ──▶│
   │        rct_code}               │ (세션 등록)
   │                                │
   │◀─── {sessionID: "xxx"} ───────│
   │                                │
```

### 2. 연결 목록 조회

```
클라이언트                          서버
   │                                │
   │──── connectList {deviceId} ──▶│
   │                                │
   │◀─── {res: "connectList", ────│
   │        cnt: N, arr: [...] }    │
```

### 3. 메시지 전송 (특정 클라이언트로)

```
클라이언트A                        서버                          클라이언트B
   │                                │                              │
   │──── MSG {com_id, rct_code, ──▶│                              │
   │        order_info}             │                              │
   │                                │──── order_info ────────────▶│
   │◀─── {res: "MSG", ─────────────│                              │
   │        toSess.deviceid}        │                              │
```

---

## 📦 NuGet 패키지 의존성

### 서버 (ws002/ws002.csproj)

```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="SuperSocket.WebSocket.Server" Version="2.0.0-beta.8" />
```

### 클라이언트 (Poscle35/Poscle35.csproj)

```xml
<Reference Include="Newtonsoft.Json, Version=13.0.0.0" />
<Reference Include="websocket-sharp, Version=1.0.2.59611" />
```

---

## ⚙️ 설정 파일

### appsettings.json (서버)

```json
{
  "serverOptions": {
    "name": "TestWebSocketServer",
    "listeners": [
      {
        "ip": "Any",
        "port": 7000
      }
    ]
  },
  "ConnectionStrings": {
    "DefaultConnection": "server=15.164.63.234;user=...;database=test_delivery_etc"
  }
}
```

---

## 🚀 빌드 및 실행

### 서버 실행

```bash
cd ws002
dotnet run
# 서버가 포트 7000에서 Listening 시작
```

### 클라이언트 실행

```bash
cd Poscle35
# Visual Studio에서 빌드 및 실행
# 또는 exe 파일 직접 실행
```

### 빌드 요구사항

| 항목 | 서버 | 클라이언트 |
|------|------|-----------|
| **IDE** | Visual Studio 2019+ | Visual Studio 2017+ |
| **SDK** | .NET Core 3.1 SDK | .NET Framework 3.5 |
| **패키지** | NuGet 복원 | packages 폴더 참조 |

---

## 📝 파일별 라인 수 요약

| 파일 | 라인 수 | 분류 |
|------|---------|------|
| ws002/Program.cs | 58 | 서버 엔트리포인트 |
| ws002/StringPackageConverter.cs | 38 | 패킷 변환 |
| ws002/Services/UserService.cs | 98 | 서비스 로직 |
| ws002/Commands/MSG.cs | 40 | 메시지 명령 |
| ws002/Commands/COMMAND_CON.cs | 38 | 연결 명령 |
| Poscle35/WScle.cs | 236 | WebSocket 클라이언트 |
| Poscle35/Form1.Designer.cs | 166 | UI 디자이너 |
| Poscle35/FormSub.cs | 96 | 폼 헬퍼 |

---

## 🔮 확장 가능성

1. **TLS/SSL 적용**: appsettings.json의 certificateOptions 설정
2. **데이터베이스 연동**: ConnectionStrings의 MySQL 연결 활용
3. **다중 서버**: 리스너 추가 및 로드밸런싱
4. **인증/인가**: JWT 토큰 기반 인증
5. **모니터링**: 로깅 시스템 (Serilog) 활성화

---

*문서 작성일: 2026-04-16*
