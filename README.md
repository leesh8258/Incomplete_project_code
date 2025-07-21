# Project Roble

+ 중단된 개발 프로젝트에서 작성했던 코드를 저장하기 위한 Repository입니다
+ 현재 Repository에는 본인이 작성한 코드만 등록하였으며 이에 대해서만 설명할 것을 밝힙니다

+ 디렉토리 구조
  + __Scripts__: 빌드 테스트에 사용되었던 코드를 모아놓은 디렉토리입니다
  + __Refectoring Scripts__: 보완점이 필요한 코드들을 재작성하고 있던 디렉토리입니다      
___

## 프로젝트 소개
<div algin="center">
  <img width="32%" src="https://github.com/user-attachments/assets/ce6e07d4-6cf9-402a-b201-9db5b7ca214c">
  <img width="32%" src="https://github.com/user-attachments/assets/e54d9ff6-c614-4ddb-b820-1968d88a89d9">
  <img width="32%" src="https://github.com/user-attachments/assets/21a6f54a-ddea-42a8-9957-0083ffc4f0f4">
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

## 리팩토링

### 1. 공격
<details><summary>리팩토링 전 공격 구조</summary>
  <pre>

  AttackBase (abstract MonoBehaviour)
  ├── MeleeAttackBase
  │   ├── MeleeFanAttack
  │   ├── MeleeRushAttack
  │   └── MeleeStraightAttack
  └── RangeAttackBase
      ├── RangeSingleShotAttack
      └── RangeHitscanShotAttack
  
  BulletBase (abstract MonoBehaviour)
  └── NormalBullet
  
  [공격 흐름]
  Enemy
  └─ AttackSequence()
      ├─ Wait (BeforeAnim)
      ├─ PerformAttack()
      └─ Wait (AfterAnim)
    
  </pre>
</details>

<details><summary>리팩토링 후 공격 구조</summary>
  <pre>
    
  EnemyAttackController (MonoBehaviour)
  ├── List<IAttackHandler>
  │   ├── DashAttack         ← MeleeAttackBase_NEW
  │   ├── AreaAttack         ← MeleeAttackBase_NEW
  │   └── StraightAttack     ← MeleeAttackBase_NEW
  ├── ShootToTarget          ← ProjectileHandler
  └── ...
  
  ProjectileBase (abstract MonoBehaviour)
  └── LaserProjectile
  
  AttackDataSO (ScriptableObject)
  ├── attackCoefficient
  ├── attackRange
  ├── hitTiming / totalDelay
  └── projectilePrefab, bulletCount, ...
  
  [공격 흐름]
  EnemyAttackController
  └─ Animator.SetTrigger("Attack")
      ├─ Wait (hitTiming)
      ├─ IAttackHandler.Execute()
      └─ Wait (delay - hitTiming)
    
  </pre>
</details>

#### 주요 리팩토링 요약

+ Interface 기반 공격 실행 분리
+ 공격데이터 SO화
+ 발사체 동작 책임 분리
+ AttackController 로직 최적화

#### 코드 변경 표
| 변경점        | 이유                         | 개선사항                    |
|-------------|------------------------------|------------------------------|
| `IAttackHandler` 인터페이스 도입 | 기존에는 AttackBase를 근접/원거리 공격으로 상속하여 구분했기 때문에, 공격 로직이 중복되거나 커스터마이징이 어려웠음 | 1. 근접/원거리 구분 없이 IAttackHandler만 구현하면 자유롭게 커스터마이징 가능<br>2. 공격 로직을 실행 단위로 분리하여 기능 확장 및 유지보수가 쉬워짐 |
| `AttackDataSO`(ScriptableObject) 도입 | 기존에는 공격 계수 등 수치를 코드 내에서 직접 수정해야 했기 때문에, 기획자가 직접 밸런스를 조정하기 어려운 구조였음 | 1. 공격 데이터를 외부화하여 기획자와의 협업 효율이 대폭 향상됨<br>2. 공격 밸런스 조정 시 SO 파일만 수정하면 되므로 유지보수와 테스트가 쉬워짐 |
| 발사체 생성/동작 책임 분리| 기존에는 발사체 생성과 방향 설정, 공격 적용까지 모두 하나의 스크립트에서 처리되어 코드 중복과 결합도가 높음 | 1. ProjectileHandler가 발사체 생성 및 방향 설정을 담당하고, ProjectileBase가 실제 동작을 처리하여 책임을 명확히 분리<br>2. 구조가 유연해져 유도탄, 폭발탄, 레이저 등 다양한 발사체 확장이 쉬워짐 |

---
### 2. 버프
<details><summary>리팩토링 전 버프 구조</summary>
</details>

<details><summary>리팩토링 전 공격 구조</summary>
</details>
