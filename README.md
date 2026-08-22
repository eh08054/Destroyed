# Destoryed(가제)

파괴된 도시에서 몬스터들과 싸우는 2D 플랫포머 게임입니다.

---

## 개발 환경

+ Engine: Unity 6
+ Language: C#
+ Project Type: 2D
+ Target Platform: Windows PC
+ Team Size: 1인
+ Genre: 2D, 플랫포머, 아포칼립스, 액션, 횡스크롤
  
---

## 데모 영상
<img width="500" height="300" alt="GamePlayDemo" src="https://github.com/user-attachments/assets/ee52a3c3-9af8-4c1d-b4c6-ef5c8a3ece2c" />

---

## 주요 기능

+ 입력에 따른 플레이어 액션(이동/공격/점프/대쉬/...)
+ 플레이어 위치에 따른 적 상태 머신(이동/추적/공격)
+ MVP 패턴 기반 인벤토리/상점 시스템
+ 오브젝트 풀, AudioMixer 기반 오디오 시스템
+ 아이템별 가중치를 가진 드롭 시스템
+ 스킬트리/버프/강화 시스템
+ ParticleSystem을 활용한 VFX 등

---

## 아키텍쳐

+ GameManager &rarr; 플레이어/적/배경의 생성 및 삭제와 데이터 저장/로드를 담당함.
+ UIManager &rarr; 최상위 패널의 생성/삭제 및 활성화/비활성화룰 담당함.
+ AudioManager &rarr; 게임의 음성을 담당함. 
+ PlayerController &rarr; 플레이어의 물리적 행위를 제어함.
+ CameraController &rarr; 카메라의 움직임을 제어함.
+ PlayerBase &rarr; 플레이어의 런타임 패시브 데이터를 저장함.
+ ShopController, InventoryController &rarr; 각자의 Model과 View를 연결 등
  
---

## 트러블슈팅

+ Skill 구현 시 ActiveSkill과 PassiveSkill의 성질이 서로 다르지만 Skill이라는 정체성은 공유해야함.<br>
  &rarr; ActiveSkill.cs, PassiveSkill.cs가 Skill.cs를 상속하도록 구현.
    
+ 람다 함수를 for문 내에서 사용 시 배열의 값 변수(slots[i] 등)를 넘길 때 최종값으로 넘기는 현상(&rarr; Closure Capture)<br>
  &rarr; var slot = slots[i] 형태로 지역변수로 복사한 후 사용하여 해결.
  
+ 공통된 상태머신을 가진 적 몬스터들의 애니메이션을 각각 만들어야 하는 문제<br>
  &rarr; Sprite Library Asset 및 Sprite Resolver를 사용해 각 상태에 따른 스프라이트셋을 등록하여 애니메이션을 간단하게 생성.
  
+ 사운드(특히 SFX) 발생 시 매번 Instantiate, Destroy를 함으로써 자원이 낭비되는 문제<br>
  &rarr; 사운드를 Object Pool 형태로 관리함. 즉, 적절한 크기의 AudioSource 배열을 미리 생성해둔 뒤 필요할 때 대여, 사용 후 반납 형태로 구현.

+ Drag & Drop, Dropdown, Button, 기타 이벤트 기반 UI 조작 시 겹치는 투명 이미지의 Raycast target이 켜져있어 기능이 제대로 작동하지 않는 문제<br>
  &rarr; 이벤트가 필요하지 않은 UI의 경우 Raycast target을 비활성화.

---

## 조작 방법

+ 방향키(&rarr; &larr;) &rarr; 플레이어 좌우 이동       
+ Z &rarr; 플레이어 공격/이중 공격 
+ X &rarr; 대쉬          
+ C &rarr; 점프/더블점프
+ C + &darr; &rarr; 하단 점프(플랫폼 위)    
+ I &rarr; 인벤토리        
+ K &rarr; 보유 스킬       
+ ESC &rarr; 패널 비활성화/일시 정지

---

## 프로젝트 로드맵  

+ 무기 교체 및 각 무기별 플레이어 스프라이트 추가(현재는 칼/총 두개만 존재)
+ 스토리/NPC 추가
+ 적 오브젝트/모션 추가
+ 신규 아이템/스킬 추가
+ 인벤토리/상점 패널 이외에도 UI의 아키텍쳐를 MVP 패턴 따르도록 기능을 분리하여 개발 효율성을 증대
+ 스테이지 추가
+ 보스 패턴 추가
+ UI 배치 최적화
+ 기타 버그 수정 등
  
