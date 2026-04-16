# 🚀 Push Server & Client - 로드맵

> 프로젝트 개선 로드맵 및 이슈 관리

---

## 📊 현재 상태 요약

| 구성요소 | 현재 상태 | 우선순위 |
|---------|---------|---------|
| **서버 (ws002)** | .NET Core 3.1, 포트 7000, SuperSocket | - |
| **클라이언트 (Poscle35)** | .NET Framework 3.5, Windows Forms | - |
| **WebSocket** | 기본 연결/메시지 전송 가능 | - |
| **문서화** | README.md, PROJECT_STRUCTURE.md | ✅ 완료 |
| **CI/CD** | 미구현 | ⚠️ 필요 |
| **보안** | TLS 미활성화, credentials 노출 | ⚠️ 위험 |

---

## 🎯 개선 로드맵

### Phase 1: 안정성 강화 (Immediate)

| 이슈 | 설명 | 난이도 | 예상 시간 |
|------|------|--------|----------|
| 🔄 **WebSocket 재연결 로직** | 연결 끊김 시 자동 재시도, 백오프 전략 | ⭐⭐ | 2-4h |
| 🛡️ **보안 강화** | credentials 제거, secrets 관리 | ⭐⭐ | 1-2h |
| 📝 **에러 처리 개선** | 예외 처리, 로깅 강화 | ⭐⭐ | 2-3h |

### Phase 2: 개발 효율성 (Short-term)

| 이슈 | 설명 | 난이도 | 예상 시간 |
|------|------|--------|----------|
| ⚙️ **CI/CD 파이프라인** | GitHub Actions로 빌드 자동화 | ⭐⭐⭐ | 4-6h |
| 🐳 **Docker 지원** | 서버 컨테이너화 | ⭐⭐ | 3-4h |
| 🔒 **TLS/SSL 적용** | WSS 프로토콜 활성화 | ⭐⭐ | 2-3h |

### Phase 3: 코드 품질 (Medium-term)

| 이슈 | 설명 | 난이도 | 예상 시간 |
|------|------|--------|----------|
| 🧪 **단위 테스트** | xUnit/NUnit 테스트 추가 | ⭐⭐⭐ | 1-2d |
| 📦 **프레임워크 업그레이드** | .NET 6/7 마이그레이션 | ⭐⭐⭐⭐ | 1-2w |
| 🔧 **SDK-style 프로젝트** | csprojmodern화 | ⭐⭐ | 2-3d |

### Phase 4: 기능 확장 (Long-term)

| 이슈 | 설명 | 난이도 | 예상 시간 |
|------|------|--------|----------|
| 🔑 **JWT 인증** | 토큰 기반 인증 시스템 | ⭐⭐⭐ | 1-2d |
| 📊 **모니터링** | Prometheus/Grafana 연동 | ⭐⭐⭐ | 2-3d |
| 🔄 **Redis 세션** | 분산 세션 스토어 | ⭐⭐⭐⭐ | 3-4d |
| ☁️ **클라우드 배포** | AWS/Azure 배포 가이드 | ⭐⭐⭐ | 2-3d |

---

## 📋 상세 개선사항

### 1. WebSocket 재연결 로직

**현재 상태**: 연결 실패 시 재시도 없음

**개선 목표**:
- 연결 끊김 감지
- 자동 재연결 (최대 5회)
- 백오프 전략 (1s, 2s, 4s, 8s...)
- 재연결 실패 시 사용자 알림

```csharp
// 개선 예시
public async Task ConnectWithRetry(int maxRetries = 5)
{
    var retryCount = 0;
    while (retryCount < maxRetries)
    {
        try
        {
            await ConnectAsync();
            return;
        }
        catch
        {
            retryCount++;
            var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
            await Task.Delay(delay);
        }
    }
}
```

### 2. 보안 강화

**현재 상태**: credentials가 appsettings.json에 하드코딩

**개선 목표**:
- secrets를 환경변수 또는 GitHub Secrets로 관리
- appsettings.json에서 실제 값 제거
- 개발용 appsettings.Development.json 사용

```json
// 개선 후
{
  "ConnectionStrings": {
    "DefaultConnection": "${DB_CONNECTION_STRING}"
  }
}
```

### 3. CI/CD 파이프라인

**현재 상태**: 수동 빌드

**개선 목표**:
- GitHub Actions로 자동 빌드/테스트
- PR 시 자동 검증
- Release 시 자동 배포

```yaml
# 서버 빌드 워크플로우 예시
name: Build Server
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '3.1.x'
      - run: dotnet build ws002/ws002.csproj
      - run: dotnet test
```

### 4. TLS/SSL 적용

**현재 상태**: ws:// (평문)

**개선 목표**:
- WSS:// 프로토콜로 전환
- 자체 서명 인증서 (개발용)
- 정식 인증서 (운영용)

### 5. 단위 테스트

**현재 상태**: 테스트 없음

**개선 목표**:
- Commands 핸들러 테스트
- Services 로직 테스트
- Integration 테스트 (WebSocket 통신)

### 6. 프레임워크 업그레이드

**현재 상태**:
- 서버: .NET Core 3.1
- 클라이언트: .NET Framework 3.5

**개선 목표**:
- 서버: .NET 6/7/8 LTS
- 클라이언트: .NET 6/7 + WinForms 또는 WPF

---

## 🏷️ 이슈 관리 규칙

### 라벨 체계

| 라벨 | 설명 | 색상 |
|------|------|------|
| `enhancement` | 새 기능 | 🟢 green |
| `bug` | 버그 수정 | 🔴 red |
| `security` | 보안 관련 | 🟠 orange |
| `ci-cd` | CI/CD 관련 | 🔵 blue |
| `documentation` | 문서화 | 🟡 yellow |
| `refactoring` | 리팩토링 | 🟣 purple |
| `good first issue` | 초보자 친화 | ⚪ white |

### 마일스톤

| 마일스톤 | 목표 | 이슈 수 |
|---------|------|--------|
| v1.2.0 | 안정성 강화 | 3 |
| v1.3.0 | 개발 효율성 | 3 |
| v2.0.0 | 코드 품질 | 3 |
| v2.1.0 | 기능 확장 | 4 |

---

## 📈 활용 시나리오

### 1. 딜리버리 푸시 시스템
```
주문 발생 → 서버 → Rider 앱 (WebSocket 푸시)
           ↓
      실시간 알림 + 주문 정보 전달
```

### 2. 포스 연동
```
포스 시스템 → 서버 → 키오스크/테이블매니저
              ↓
         주문 동기화 + 결제 연동
```

### 3. IoT 디바이스 관리
```
IoT 디바이스 → 서버 → 모니터링 대시보드
              ↓
         실시간 상태 + 명령下发
```

### 4. 채팅/알림 서비스
```
모바일 앱 ↔ 서버 (WebSocket 양방향 통신)
           ↓
      실시간 메시지 + 푸시 알림
```

---

*Last Updated: 2026-04-16*
