# Project Roble

+ 중단된 개발 프로젝트에서 작성했던 코드를 저장하기 위한 Repository입니다
+ 현재 Repository에는 본인이 작성한 코드만 등록하였으며 이에 대해서만 설명할 것을 밝힙니다

+ 디렉토리 구조
  + __Scripts__: 빌드 테스트에 사용되었던 코드를 모아놓은 디렉토리입니다
  + __Refectoring Scripts__: 보완점이 필요한 코드들을 재작성하고 있던 디렉토리입니다      
___

## 프로젝트 소개
<div algin="center">
  <img width="33%" src="https://github.com/user-attachments/assets/ce6e07d4-6cf9-402a-b201-9db5b7ca214c">
  <img width="33%" src="https://github.com/user-attachments/assets/e54d9ff6-c614-4ddb-b820-1968d88a89d9">
  <img width="33%" src="https://github.com/user-attachments/assets/21a6f54a-ddea-42a8-9957-0083ffc4f0f4">
  <div align="center">
    <p><개발 중 이미지></p>
  </div>
</div>

__1. 장르__
   + 어드벤처 / 디펜스 / 샌드박스

__3. 플랫폼__
   + Unity 3D / 6000.0.49f1 LTS

__3. 개발언어__
   + C#

__4. 개발기간__
   + 2025.01 ~ 2025.05 (프로젝트 중단)

## 담당 기능 및 관련 스크립트
+ 현재 설명하는 스크립트는 모두 Scripts 파일 기준입니다

### 1. 몬스터
| 기능        | 설명                         | 관련 스크립트 경로 및 파일명 |
|-------------|------------------------------|-------------------------------|
| 몬스터 AI   | FSM 기반의 적 캐릭터 AI 구현 | `HealthEntity/HealthyEntity.cs`<br>`HealthEntity/Character.cs`<br>`HealthEntity/Enemy.cs`<br>`HealthEntity/Enemies/Enemy_0_000.cs ~ Enemy_0_006.cs` |
| 몬스터 애니메이션 | Animator를 활용한 몬스터 행동 애니메이션 구현 및 몬스터 State에 따른 애니메이션 재생| `HealthEntity/Enemies/State` |
| 적 탐지 및 길 찾기 | Collider 감지를 통한 적 탐지 및 A* 알고리즘을 활용한 네비게이션 기능 | `HealthEntity/Enemies/Detect`<br>`Navigation/AStarPathfinder.cs`<br>`HealthEntity/Enemies/Move` |
| 몬스터 공격 | 몬스터 패턴에 따라 다양한 공격이 나올 수 있도록 기능 구현 | `Attack` |
| 스프라이트 | 몬스터가 바라보는 방향 및 공격방향에 따라 스프라이트 관리 | `HealthEntity/Sprite/SpriteManager.cs` |
| 버프 관리 | 몬스터 버프, 디버프 기능 구현 | `HealthEntity/Buff` |
| 스포너 | 몬스터 소환 조건을 설정할 수 있는 스포너 기능 구현 | `HealthEntity/Enemies/Spawner` |
<br>

### 2. 스탯
| 기능      | 설명                                          | 관련 스크립트 경로 및 파일명 |
|-----------|-----------------------------------------------|-------------------------------|
|공용 스탯 | 몬스터, 블럭, 플레이어 공통으로 사용가능한 스탯 변수 | `HealthEntity/Stat` |
<br>

### 3. 유틸리티
| 기능      | 설명                                          | 관련 스크립트 경로 및 파일명 |
|-----------|-----------------------------------------------|-------------------------------|
|PriorityQueue | 적 탐지 중 우선순위가 높은 적을 탐지하기 위한 유틸리티 함수 작성 | `Utilities/PriorityQueue.cs` |
|CSV Parser | CSV 파일 내용을 Unity 내 SO로 변환하여 매핑하기 위한 파서 | `Utilities/CsvParser.cs` |
<br>

## 리팩토링 코드 비교
