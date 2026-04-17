# 📜 프로젝트 개발 이력 (HISTORY.md)

이 파일은 프로젝트의 주요 변경 사항, 빌드, 커밋, 푸시 이력을 기록합니다.

---

## 📅 2026-04-17

### 🚀 GitHub Actions 배포 워크플로우 수정
- **현상**: GitHub Actions에서 `release` 작업 중 파일 경로를 찾지 못하거나 아티팩트가 비어 있는 문제 발생.
- **원인**:
    1. `upload-artifact` 시 `working-directory` 설정으로 인해 실제 파일이 있는 경로와 업로드 경로가 불일치함.
    2. `ubuntu-latest` 환경에서 `powershell`의 `Compress-Archive`를 사용했으나 경로 및 환경 차이로 비정상 작동 가능성.
    3. 아티팩트 다운로드 후 압축 시 경로 참조 오류.
- **수정 사항**:
    - `build-server`, `build-client`의 `publish` 출력을 로컬 `publish/` 폴더로 변경.
    - `upload-artifact` 경로를 루트 기준(`ws002/publish/`, `Poscle35/publish/`)으로 명시하여 정확한 파일 업로드 보장.
    - `release` 작업에서 `powershell` 대신 표준 `zip` 명령어를 사용하여 호환성 및 안정성 확보.
    - 다운로드 및 압축 경로를 `release_artifacts/`로 통일하여 명확하게 관리.
- **결과**: 수정 사항 커밋 및 푸시 완료. GitHub Actions 재검증 대기 중.

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
