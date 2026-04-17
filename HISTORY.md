# 📜 프로젝트 개발 이력 (HISTORY.md)

이 파일은 프로젝트의 주요 변경 사항, 빌드, 커밋, 푸시 이력을 기록합니다.

---

## 📅 2026-04-17

### 🚀 GitHub Actions 배포 워크플로우 2차 수정
- **현상**: `v2.4.3` 태그 배포 실패.
- **원인 분석**:
    1. 클라이언트(`Poscle35`)가 .NET 9(SDK-style)임에도 불구하고 `msbuild`와 `nuget restore`를 사용함.
    2. `nuget restore Poscle35.sln` 실행 시 해당 폴더에 `.sln` 파일이 없어 오류 발생 가능성.
    3. `zip` 명령어의 와일드카드(`*`) 처리 문제 가능성.
- **수정 사항**:
    - 클라이언트 빌드를 `dotnet publish` 명령어로 교체하여 .NET 9 환경에 최적화.
    - 클라이언트 작업에서 `setup-dotnet` 단계 추가 및 `dotnet restore` 사용.
    - `release` 작업에서 `zip` 설치 보장 및 압축 명령어 단순화 (디렉토리 직접 지정).
- **결과**: 수정 사항 커밋 및 푸시 완료. 태그 `v2.4.4` 생성으로 워크플로우 재실행.

### 🔧 기타 수정 사항 (Git 이력 기반)
- `fix: Add release workflow with proper permissions` (v2.4.2)
- `fix: add contents:write permission to release job`
- `fix: correct Linux ZIP path for artifact upload` (v2.4.1)
- `fix: use Timer alias to resolve ambiguity`
- `fix: add GlobalUsings.cs to resolve Timer ambiguity`

---

## 📅 2026-04-16
- 프로젝트 로드맵 및 구조 문서 최신화 완료.
- 서버(.NET Core 3.1) 및 클라이언트(.NET Framework 3.5) 마이그레이션 및 연동 테스트 완료.
