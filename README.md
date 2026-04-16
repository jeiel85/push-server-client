# 🔌 Push Server & Client

> WebSocket 기반 실시간 푸시 알림 시스템 - 서버와 Windows Forms 클라이언트 샘플 프로젝트

![.NET](https://img.shields.io/badge/.NET-Core%203.1-512BD4?style=flat-square&logo=.net)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-3.5-512BD4?style=flat-square&logo=.net)
![C#](https://img.shields.io/badge/C%23-9.0-239120?style=flat-square&logo=c-sharp)
![WebSocket](https://img.shields.io/badge/WebSocket-SuperSocket-006400?style=flat-square)
![Status](https://img.shields.io/badge/status-active-brightgreen?style=flat-square)

---

## 📖 목차

- [프로젝트 개요](#-프로젝트-개요)
- [기술 스택](#-기술-스택)
- [프로젝트 구조](#-프로젝트-구조)
- [빠른 시작](#-빠른-시작)
- [통신 프로토콜](#-통신-프로토콜)
- [주요 기능](#-주요-기능)
- [스크린샷](#-스크린샷)
- [향후 확장](#-향후-확장)

---

## 📋 프로젝트 개요

이 프로젝트는 **WebSocket 기반 양방향 실시간 통신**을 구현한 푸시 서버 & 클라이언트 샘플입니다.

### 주요 특징

- ✅ **실시간 양방향 통신** - WebSocket 기반 즉각적인 메시지 교환
- ✅ **命令 기반 아키텍처** - 확장 가능한 명령 핸들러 시스템
- ✅ **다중 클라이언트 관리** - 세션 기반 사용자/디바이스 관리
- ✅ **손쉬운 테스트** - Windows Forms 기반 그래픽 클라이언트 제공
- ✅ **주문 정보 푸시** - 딜리버리/포스 환경에 최적화된 데이터 모델

### 사용 사례

| 시나리오 | 설명 |
|----------|------|
| 🛵 **딜리버리 푸시** | 배달 주문 정보를 Rider 앱으로 실시간 전송 |
| 🏪 **포스 연동** | POS 시스템과 외부 디바이스 간 주문 동기화 |
| 📱 **모바일 알림** | 실시간 푸시 알림 서비스 백엔드 |
| 💬 **채팅 시스템** | 기본 WebSocket 통신 Infrastructure |

---

## 🛠 기술 스택

### 서버 (.NET Core 3.1)

| 기술 | 버전 | 용도 |
|------|------|------|
| **.NET Core** | 3.1 | 런타임 |
| **SuperSocket** | 2.0.0-beta.8 | WebSocket 서버 프레임워크 |
| **Newtonsoft.Json** | 13.0.3 | JSON 직렬화/역직렬화 |

### 클라이언트 (.NET Framework 3.5)

| 기술 | 버전 | 용도 |
|------|------|------|
| **.NET Framework** | 3.5 | 런타임 |
| **Windows Forms** | - | GUI 프레임워크 |
| **websocket-sharp** | 1.0.3-rc11 | WebSocket 클라이언트 라이브러리 |
| **Newtonsoft.Json** | 13.0.3 | JSON 처리 |

### 개발 도구

| 도구 | 추천 버전 |
|------|----------|
| **IDE** | Visual Studio 2019+ |
| **.NET Core SDK** | 3.1.x |
| **.NET Framework** | 3.5 |
| **빌드 도구** | MSBuild |

---

## 📁 프로젝트 구조

```
push-server-client/
│
├── 📂 ws002/                          # 🔷 WebSocket Server
│   ├── Program.cs                     # Entry point
│   ├── StringPackageConverter.cs      # Packet converter
│   ├── appsettings.json               # Configuration
│   ├── Commands/                     # Command handlers
│   │   ├── noneProc.cs              # Default handler
│   │   ├── connectList.cs           # Connection list
│   │   ├── MSG.cs                  # Message routing
│   │   └── COMMAND_CON.cs           # Connection setup
│   ├── Services/                     # Business logic
│   │   ├── UserSession.cs          # Session management
│   │   └── UserService.cs          # User management
│   └── Models/                       # Data models
│       ├── MsgBin.cs                # Message model
│       └── LogBin.cs                # Log model
│
├── 📂 Poscle35/                        # 🟢 Windows Forms Client
│   ├── Program.cs                    # Entry point
│   ├── Form1.cs / Form1.Designer.cs # Main UI
│   ├── FormSub.cs                   # UI helpers
│   ├── WScle.cs                    # WebSocket wrapper
│   └── Models/                      # Data models
│
├── 📄 ws002.sln                      # Solution file
├── 📄 README.md                      # This file
└── 📄 PROJECT_STRUCTURE.md          # Detailed architecture
```

---

## 🚀 빠른 시작

### 사전 요구사항

- Windows OS
- Visual Studio 2019+
- .NET Core 3.1 SDK
- .NET Framework 3.5

### 서버 실행

```bash
# 1. 서버 프로젝트로 이동
cd ws002

# 2. NuGet 패키지 복원
dotnet restore

# 3. 서버 실행 (포트 7000)
dotnet run
```

서버가 실행되면 다음 로그가 표시됩니다:
```
info: SuperSocket.WebSocket.WebSocketServer[0]
      Started SuperSocket[SuperSocket.WebSocket.WebSocketServer]
info: SuperSocket.SocketBase.BaseServer[0]
      The server TestWebSocketServer has been started on port 7000
```

### 클라이언트 실행

```bash
# Visual Studio에서 Poscle35 프로젝트 열기
# F5 또는 Ctrl+F5로 실행
```

또는 빌드된 exe 파일 실행:
```bash
Poscle35/bin/Debug/Poscle35.exe
```

### 연결 테스트 순서

1. **서버 먼저 실행** (포트 7000)
2. **클라이언트 실행**
3. **클라이언트 UI에서 테스트**:
   - `WS connect` → 연결
   - `WS 1st con` → 세션 등록
   - `WS connectList` → 연결 목록 확인
   - `WS to send` → 메시지 전송

---

## 📡 통신 프로토콜

### 패킷 구조

모든 메시지는 JSON 형식으로 교환됩니다:

```json
{
  "Key": "COMMAND_NAME",
  "Body": { ... }
}
```

### 지원 명령

| 명령 | 방향 | 설명 |
|------|------|------|
| `CON` | C → S | 클라이언트 연결 및 세션 등록 |
| `connectList` | C → S | 연결된 클라이언트 목록 조회 |
| `MSG` | C → S | 특정 클라이언트로 메시지 전송 |
| `noneProc` | C → S | 기본 처리 (로깅용) |

### 1. 연결 및 세션 등록 (CON)

**Request:**
```json
{
  "Key": "CON",
  "Body": {
    "deviceId": "123456",
    "com_id": "demo",
    "rct_code": "1002"
  }
}
```

**Response:**
```json
{
  "sessionID": "SR_001D7F9E"
}
```

### 2. 연결 목록 조회 (connectList)

**Request:**
```json
{
  "Key": "connectList",
  "Body": {
    "deviceId": "123456"
  }
}
```

**Response:**
```json
{
  "res": "connectList",
  "cnt": 2,
  "arr": ["device001", "device002"]
}
```

### 3. 메시지 전송 (MSG)

**Request:**
```json
{
  "Key": "MSG",
  "Body": {
    "com_id": "demo",
    "rct_code": "1002",
    "order_info": "{\"AO_ID\": 473449, ...}"
  }
}
```

**Response:**
```json
{
  "res": "MSG",
  "toSess": "device_id"
}
```

---

## ⚡ 주요 기능

### 서버 기능

| 기능 | 설명 |
|------|------|
| 🔌 **WebSocket 서버** | 포트 7000에서 다중 클라이언트 연결 수락 |
| 📋 **세션 관리** | deviceid, com_id, rct_code 기반 세션 추적 |
| 📨 **메시지 라우팅** | com_id + rct_code로 특정 클라이언트寻址 |
| 📊 **연결 목록** | 현재 연결된 모든 클라이언트 조회 |
| 📝 **로깅** | 연결/해제 이벤트 로깅 |

### 클라이언트 기능

| 기능 | 설명 |
|------|------|
| 🖥️ **GUI 인터페이스** | Windows Forms 기반 직관적인 UI |
| 🔗 **연결 관리** | 원-click 연결/연결 해제 |
| 📤 **요청 전송** | 미리 정의된 명령 템플릿 전송 |
| 📥 **응답 표시** | 요청/응답 로그 별도 표시 |
| 🛡️ **스레드 세이프** | 비동기 콜백에서의 안전한 UI 업데이트 |

### 데이터 모델

**주문 정보 (order_info):**
```json
{
  "AO_ID": 473449,
  "MEMBER_ID": "1000000000000000",
  "MEMBER_NM": "소비자",
  "AMT": 8800,
  "PAYMENT_TYPE": "11105500",
  "DELIVERY_STATUS": "31",
  "ORDER_TYPE": "D",
  "LAND_ADDRESS": "서울특별시 종로구...",
  "ROAD_ADDRESS": "서울특별시 종로구 율곡로...",
  "LATITUDE": "37.5768897288",
  "LONGITUDE": "126.9889239442",
  "MESSAGE": "도착해서 전화 주시면 내려갈게요."
}
```

---

## 📸 스크린샷

### 클라이언트 UI 레이아웃

```
┌────────────────────────────────────────────────────────────────┐
│  Server: [로컬 ▼]    SessionID: xxxx-xx-xx      MyID: 0000     │
├──────────┬──────────────────────┬──────────────────────────────┤
│ Menu     │ Request Log          │ Response Log                  │
│          │                      │                               │
│ WS connect│ {                  │ {                             │
│ WS Disc.. │   "Key": "CON",     │   "sessionID": "SR_001D7F9E" │
│ WS 1st con│   "Body": {...}     │ }                             │
│ WS conn.. │ }                   │                               │
│ WS to send│                     │ {                             │
│          │                      │   "res": "connectList",      │
│          │                      │   "cnt": 2                    │
│          │                      │ }                             │
│          │                      │                               │
├──────────┴──────────────────────┴──────────────────────────────┤
│                                          [SEND]      [CLEAR]    │
└────────────────────────────────────────────────────────────────┘
```

### 통신 흐름

```
┌─────────────┐                              ┌─────────────┐
│   Client    │  ─────── CON {deviceId} ──▶ │   Server    │
│  (Poscle35) │  ◀───── {sessionID} ─────── │  (ws002)    │
│             │                              │             │
│             │  ── connectList {deviceId} ─▶│             │
│             │  ◀── {cnt, arr[]} ──────────│             │
│             │                              │             │
│             │  ── MSG {com_id, order} ──▶ │             │
│             │  ◀── {res: "MSG"} ──────────│             │
└─────────────┘                              └─────────────┘
```

---

## 🔮 향후 확장

### 즉시 적용 가능

| 확장 | 설명 | 난이도 |
|------|------|--------|
| 🔒 **TLS/SSL** | WSS 프로토콜 적용 | ⭐⭐ |
| 📱 **REST API** | HTTP 엔드포인트 추가 | ⭐⭐ |
| 📊 **모니터링** | 실시간 대시보드 | ⭐⭐⭐ |

### 추가 개발 필요

| 확장 | 설명 | 난이도 |
|------|------|--------|
| 🗄️ **DB 연동** | MySQL 주문 저장 | ⭐⭐⭐ |
| 🔑 **JWT 인증** | 토큰 기반 인증 | ⭐⭐⭐ |
| 🔄 **Redis 캐시** | 세션 스토어 분리 | ⭐⭐⭐⭐ |
| 🐳 **Docker** | 컨테이너화 | ⭐⭐ |
| ☁️ **클라우드 배포** | AWS/Azure 배포 | ⭐⭐⭐ |

### 권장 개선사항

1. **에러 처리 강화** - 재연결 로직, 타임아웃 처리
2. **로깅 시스템** - Serilog 연동, 파일/DB 로깅
3. **API 문서화** - Swagger/OpenAPI 적용
4. **단위 테스트** - xUnit/NUnit 테스트 추가
5. **CI/CD** - GitHub Actions 빌드 자동화

---

## 📂 관련 문서

| 문서 | 설명 |
|------|------|
| [PROJECT_STRUCTURE.md](./PROJECT_STRUCTURE.md) | 상세 프로젝트 구조 및 아키텍처 |
| [ws002/Program.cs](./ws002/Program.cs) | 서버 엔트리포인트 |
| [Poscle35/WScle.cs](./Poscle35/WScle.cs) | WebSocket 클라이언트 구현 |
| [ws002/appsettings.json](./ws002/appsettings.json) | 서버 설정 |

---

## 📜 라이선스

MIT License

---

## 👤 작성자

**Project**: push-server-client  
**Last Updated**: 2026-04-16

---

<p align="center">
  <sub>Built with ❤️ using .NET & WebSocket</sub>
</p>
