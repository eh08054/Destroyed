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
+ SettingsController, ShopController, InventoryController, etc... &rarr; 각자의 UI 제어 
  
---

## 조작 방법

+ 방향키(&rarr; &larr;) &rarr; 플레이어 좌우 이동       
+ Z &rarr; 플레이어 공격/이중 공격 
+ X &rarr;           대쉬          
+ C &rarr;       점프/더블점프
+ C + &darr; &rarr; 하단 점프(플랫폼 위)    
+ I &rarr;        인벤토리        
+ K &rarr;         보유 스킬       
+ ESC &rarr; 패널 비활성화/일시 정지

---

## 프로젝트 로드맵  

+ 무기 교체 및 각 무기별 플레이어 스프라이트 추가(현재는 칼/총 두개만 존재)
+ 스토리/NPC 추가
+ 적 오브젝트/모션 추가
+ 신규 아이템/스킬 추가
+ 인벤토리/상점 패널 이외에도 UI의 아키텍쳐를 MVP 패턴 따르도록 기능을 분리하여 개발 효율성을 증대 
  
