# 🔌 Push Server & Client

> WebSocket 기반 실시간 푸시 알림 시스템 - 서버와 Windows Forms 클라이언트 샘플 프로젝트

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat-square&logo=.net)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=c-sharp)
![WebSocket](https://img.shields.io/badge/WebSocket-ASP.NET%20Core-006400?style=flat-square)
![Status](https://img.shields.io/badge/status-active-brightgreen?style=flat-square)
![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)

---

## 📖 목차

- [프로젝트 개요](#-프로젝트-개요)
- [기술 스택](#-기술-스택)
- [프로젝트 구조](#-프로젝트-구조)
- [빠른 시작](#-빠른-시작)
- [Docker 지원](#-docker-지원)
- [Kubernetes 배포](#-kubernetes-배포)
- [통신 프로토콜](#-통신-프로토콜)
- [REST API](#-rest-api)
- [메트릭 모니터링](#-메트릭-모니터링)
- [주요 기능](#-주요-기능)
- [릴리즈](#-릴리즈)
- [향후 확장](#-향후-확장)

---

## 📋 프로젝트 개요

이 프로젝트는 **WebSocket 기반 양방향 실시간 통신**을 구현한 푸시 서버 & 클라이언트 샘플입니다.

### 주요 특징

- ✅ **실시간 양방향 통신** - WebSocket 기반 즉각적인 메시지 교환
- ✅ **명령 기반 아키텍처** - 확장 가능한 명령 핸들러 시스템
- ✅ **다중 클라이언트 관리** - 세션 기반 사용자/디바이스 관리
- ✅ **손쉬운 테스트** - Windows Forms 기반 그래픽 클라이언트 제공
- ✅ **Docker/Kubernetes 지원** - 컨테이너 및 오케스트레이션 지원
- ✅ **모니터링** - Prometheus 메트릭, Health Check
- ✅ **Rate Limiting** - 클라이언트별 요청 제한
- ✅ **Graceful Shutdown** - 안전한 서버 종료

### 사용 사례

| 시나리오 | 설명 |
|----------|------|
| 🛵 **딜리버리 푸시** | 배달 주문 정보를 Rider 앱으로 실시간 전송 |
| 🏪 **포스 연동** | POS 시스템과 외부 디바이스 간 주문 동기화 |
| 📱 **모바일 알림** | 실시간 푸시 알림 서비스 백엔드 |
| 💬 **채팅 시스템** | 기본 WebSocket 통신 Infrastructure |

---

## 🛠 기술 스택

### 서버 (.NET 9)

| 기술 | 버전 | 용도 |
|------|------|------|
| **.NET** | 9.0 | 런타임 |
| **ASP.NET Core WebSocket** | Built-in | WebSocket 서버 |
| **Serilog** | 8.0.1 | 구조화된 로깅 |
| **Swagger** | 6.5.0 | API 문서화 |
| **prometheus-net** | 8.2.1 | 메트릭 수집 |

### 클라이언트 (.NET 9)

| 기술 | 버전 | 용도 |
|------|------|------|
| **.NET** | 9.0 | 런타임 |
| **Windows Forms** | Built-in | GUI 프레임워크 |
| **System.Net.WebSockets** | Built-in | WebSocket 클라이언트 |
| **Newtonsoft.Json** | 13.0.3 | JSON 처리 |

### 개발 도구

| 도구 | 추천 버전 |
|------|----------|
| **IDE** | Visual Studio 2022+ |
| **.NET SDK** | 9.0.x |
| **Docker** | 24.0+ |
| **kubectl** | 1.28+ |

---

## 📁 프로젝트 구조

```
push-server-client/
│
├── 📂 ws002/                          # 🔷 WebSocket Server (.NET 9)
│   ├── Program.cs                      # Entry point + 미들웨어
│   ├── Middleware/
│   │   └── RateLimitingMiddleware.cs   # Rate Limiting
│   ├── Services/
│   │   ├── UserService.cs              # 세션 관리
│   │   └── WebSocketSession.cs         # WebSocket 세션
│   ├── Models/                        # 데이터 모델
│   ├── appsettings.json                # 프로덕션 설정
│   └── ws002.csproj
│
├── 📂 Poscle35/                        # 🟢 Windows Forms Client (.NET 9)
│   └── ...
│
├── 📂 k8s/                            # ☸️ Kubernetes 매니페스트
│   ├── deployment.yaml                  # Deployment + Service + HPA
│   ├── configmap.yaml                 # ConfigMap
│   └── ingress.yaml                    # Ingress
│
├── 📄 Dockerfile                       # Docker 빌드
├── 📄 docker-compose.yml               # Docker Compose
├── 📄 README.md                        # 이 파일
└── 📄 PROJECT_STRUCTURE.md             # 상세 아키텍처 문서
```

---

## 🚀 빠른 시작

### 사전 요구사항

- Windows/Linux/macOS
- .NET 9 SDK
- Visual Studio 2022+ (선택)

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
info: Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager[]
      User profile is available. Using 'C:\Users\...\' for temporary key storage.
info: Microsoft.AspNetCore.Server.Kestrel[]
      Start listening on http://0.0.0.0:7000
=== WebSocket Push Server Starting on port 7000 ===
```

### 클라이언트 실행

```bash
# Visual Studio에서 Poscle35 프로젝트 열기
# F5 또는 Ctrl+F5로 실행
```

---

## 🐳 Docker 지원

### Dockerfile 빌드

```bash
# Docker 이미지 빌드
docker build -t push-server:latest .

# 컨테이너 실행
docker run -d -p 7000:7000 --name push-server push-server:latest
```

### Docker Compose

```bash
# 모든 서비스 실행
docker-compose up -d

# 로그 확인
docker-compose logs -f push-server

# 중지
docker-compose down
```

### 환경 변수

| 변수 | 기본값 | 설명 |
|------|--------|------|
| `ASPNETCORE_ENVIRONMENT` | Production | 실행 환경 |
| `ASPNETCORE_URLS` | http://+:7000 | 바인딩 URL |

---

## ☸️ Kubernetes 배포

### 네임스페이스 생성

```bash
kubectl apply -f k8s/configmap.yaml
```

### 배포

```bash
kubectl apply -f k8s/deployment.yaml

# 상태 확인
kubectl get pods -n push-system
kubectl get svc -n push-system
kubectl get hpa -n push-system
```

### Ingress 적용

```bash
kubectl apply -f k8s/ingress.yaml
```

### 헬스체크 확인

```bash
# Liveness Probe
kubectl exec -it <pod-name> -n push-system -- curl http://localhost:7000/health/live

# Readiness Probe
kubectl exec -it <pod-name> -n push-system -- curl http://localhost:7000/health/ready
```

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

## 📚 REST API

### Health Check

| 엔드포인트 | 설명 |
|-----------|------|
| `GET /health` | 기본 health check |
| `GET /health/live` | Liveness probe |
| `GET /health/ready` | Readiness probe |

**응답 예시:**
```json
{
  "status": "Healthy",
  "checks": [
    { "name": "self", "status": "Healthy", "description": "Server is running" },
    { "name": "websocket", "status": "Healthy", "description": "Active connections: 5" }
  ]
}
```

### Swagger/OpenAPI

- **URL**: `http://localhost:7000/swagger`
- Interactive API 문서 및 테스트 인터페이스 제공

---

## 📊 메트릭 모니터링

### Prometheus 엔드포인트

- **URL**: `http://localhost:7000/metrics`

### 수집 메트릭

| 메트릭 | 타입 | 설명 |
|--------|------|------|
| `websocket_connections_active` | Gauge | 활성 연결 수 |
| `websocket_messages_received_total` | Counter | 수신된 메시지 수 |
| `websocket_messages_sent_total` | Counter | 전송된 메시지 수 |
| `websocket_connection_duration_seconds` | Histogram | 연결 시간 |
| `http_requests_total` | Counter | HTTP 요청 수 |
| `http_request_duration_seconds` | Histogram | 요청 처리 시간 |

### Prometheus 설정

```yaml
scrape_configs:
  - job_name: 'push-server'
    static_configs:
      - targets: ['push-server:7000']
    metrics_path: '/metrics'
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
| 📝 **로깅** | Serilog + 파일 로깅 (Rolling) |
| 💓 **Health Check** | Kubernetes 프로브 지원 |
| 📈 **메트릭** | Prometheus 메트릭 수집 |
| 🚦 **Rate Limiting** | IP별 요청 제한 (1000 req/min) |
| 🔄 **Graceful Shutdown** | 30초 타임아웃 |
| 📚 **API 문서화** | Swagger/OpenAPI |

### 클라이언트 기능

| 기능 | 설명 |
|------|------|
| 🖥️ **GUI 인터페이스** | Windows Forms 기반 직관적인 UI |
| 🔗 **연결 관리** | 원-click 연결/연결 해제 |
| 📤 **요청 전송** | 미리 정의된 명령 템플릿 전송 |
| 📥 **응답 표시** | 요청/응답 로그 별도 표시 |

---

## 📦 데이터 모델

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

## 📦 릴리즈

| 버전 | 날짜 | 변경사항 |
|------|------|----------|
| [v2.1.0](https://github.com/jeiel85/push-server-client/releases/tag/v2.1.0) | 2026-04-16 | 코드 품질 개선 (Health Check, Swagger, Prometheus, Graceful Shutdown) |
| [v2.0.0](https://github.com/jeiel85/push-server-client/releases/tag/v2.0.0) | 2026-04-16 | .NET 9 마이그레이션 |
| [v1.1.1](https://github.com/jeiel85/push-server-client/releases/tag/v1.1.1) | 2026-04-16 | UI 개선 |

### 릴리즈 애셋

각 릴리즈에는 Windows x64 실행 파일이 포함되어 있습니다:
- `ws002-v{x.y.z}-win-x64.zip`

---

## 🔮 향후 확장

### 진행 중인 항목

| 확장 | 설명 | 상태 |
|------|------|------|
| 🔐 **JWT 인증** | 토큰 기반 인증 시스템 | 📋 예정 |
| 🔄 **Redis 세션** | 분산 세션 스토어 | 📋 예정 |
| 🔒 **메시지 암호화** | AES-256 메시지 암호화 | 📋 예정 |
| 📦 **NuGet SDK** | .NET 클라이언트 SDK | 📋 예정 |

### 추가 개발 필요

| 확장 | 설명 | 난이도 |
|------|------|--------|
| 🗄️ **DB 연동** | MySQL 주문 저장 | ⭐⭐⭐ |
| 📊 **모니터링 대시보드** | Grafana 대시보드 템플릿 | ⭐⭐⭐ |
| ☁️ **클라우드 배포** | AWS/Azure 배포 가이드 | ⭐⭐⭐ |

---

## 📂 관련 문서

| 문서 | 설명 |
|------|------|
| [PROJECT_STRUCTURE.md](./PROJECT_STRUCTURE.md) | 상세 프로젝트 구조 및 아키텍처 |
| [ws002/Program.cs](./ws002/Program.cs) | 서버 엔트리포인트 |
| [ws002/Services/UserService.cs](./ws002/Services/UserService.cs) | 세션 관리 |
| [Dockerfile](./Dockerfile) | Docker 빌드 설정 |
| [k8s/deployment.yaml](./k8s/deployment.yaml) | Kubernetes 배포 |

---

## 📜 라이선스

MIT License

---

## 👤 작성자

**Project**: push-server-client
**Version**: 2.1.0
**Last Updated**: 2026-04-16

---

<p align="center">
  <sub>Built with ❤️ using .NET 9 & WebSocket</sub>
</p>
