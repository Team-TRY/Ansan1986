# 🎮안산1986🎮
> **개발기간: 2023.04.10 ~ 2023.06.10**
</br>

## 프로젝트 소개
유니티 엔진을 활용하여 개발한 VR 버스 시뮬레이션 게임</br>
>1980년대 안산의 버스 운전사가 된 플레이어는 안전하고 빠르게 버스를 운전해야 합니다!
</br>

## 사용자 매뉴얼
시스템 요구사항
- 운영체제: Windows 10 이상 또는 MacOS 10.12 이상
-	프로세서: Intel i5 이상
-	메모리: 8GB RAM
-	그래픽: NVIDIA GTX 660 이상
-	저장공간: 2GB 이상의 여유 공간
</br>

설치 방법
- Unity Hub 설치.
- 레포지토리 클론: git clone https://github.com/Team-TRY/Ansan1986.git
- Unity에서 프로젝트 열기.
</br>

사용 방법
-	프로젝트를 열고, MainMenu 씬을 로드합니다.
-	VR 헤드셋을 연결하고, Play 버튼을 눌러 실행합니다.
-	(가상 시뮬레이터가 세팅 되어 있어 VR을 연결하지 않고도 사용 가능합니다.)
-	VR 컨트롤러를 사용하여 게임 내에서 이동하고 상호작용합니다.
</br>

## 플레이 매뉴얼
[메인 메뉴 씬] 게임을 실행하면 메인 씬이 나타납니다. 메인 씬에서는 다음과 같은 버튼이 있습니다.
-	게임 시작: 이 버튼을 클릭하면 게임이 시작됩니다. 인게임 씬으로 이동하여 버스 운행을 시작할 수 있습니다.
-	리더보드: 이 버튼을 클릭하면 리더보드 화면으로 이동합니다. 여기서 다른 플레이어들의 점수를 확인할 수 있습니다.
-	게임 종료: 이 버튼을 클릭하면 팝업과 함께 게임이 종료됩니다.
![image](https://github.com/Team-TRY/Ansan1986/assets/111439484/1d22bf04-7ad4-487f-abfe-cae226babaf9)

</br>

[인게임 씬] 인게임 씬은 실제 버스 운행을 시뮬레이션하는 화면입니다. 다양한 기능과 인터페이스가 포함되어 있습니다.

운행 인터페이스
- 미니맵: 현재 위치와 목적지를 표시합니다.
- 시간: 현재 시간을 표시하며, 정해진 시간 내에 운행을 마쳐야 합니다.
- 속도계: 현재 버스의 속도를 표시합니다.
- 대화 패널: 안내양 NPC의 대사가 출력됩니다.

버스 컨트롤러
- 기어 레버: 우측 레버를 조작하여 전진, 정지, 후진 상태를 자동으로 설정 가능합니다.
- 핸들: 핸들 오브젝트와 상호작용하여 돌리면 차체가 회전합니다.
- 기타 버스 내부 요소: 와이퍼, 각종 조명, 라디오 등의 토글이 가능합니다.

게임 플레이 방법
- 좌측 미니맵에 표시되는 정거장을 따라 승객을 태우고 내리면서 최종 종착역까지 도착해야합니다.
- 시간 내에 도착해야 하며 운행 중간의 장애물과 충돌 시 시간이 줄어듭니다.
- 게임 종료 후 이름을 입력하면 점수가 리더보드에 저장됩니다.
![image](https://github.com/Team-TRY/Ansan1986/assets/111439484/88447586-4af0-49af-9f39-70e5ac0616de)
</br>

## 사용 기술
- 전략 패턴을 활용한 버스 내부 기능 컨트롤 시스템
- 상태 패턴을 활용한 정류장 컷씬 시스템
- 유니티 NavMesh를 활용한 길찾기 네비게이션 시스템
- TimeLine을 활용한 컷씬 세팅
- XRInteractionToolKit을 활용한 VR 인터랙션 및 버스 컨트롤
- Json 저장 및 로드 기반 로컬 리더보드 시스템
- ScriptableObject를 활용한 대사 시스템
- 델리게이트와 제네릭 메서드를 활용한 UI매니저
- 실시간 점수 계산 및 출력 시스템
</br>

## 주요 기능 클래스 다이어그램

![image](https://github.com/Team-TRY/Ansan1986/assets/111439484/3d24e521-7137-427c-85e3-445ecde1f477)

유틸리티
- ScenesManager: 씬의 전환과 관련된 모든 작업을 관리
- UIManager: UI 요소들의 활성화와 비활성화를 관리
- SoundManager: 배경음악과 효과음을 관리
- UIPopup: 팝업 창의 내용을 설정하고 버튼 클릭 이벤트를 처리
</br>

![image](https://github.com/Team-TRY/Ansan1986/assets/111439484/0519a002-1714-486d-a8b3-eb9a40e1a56e)

점수 및 리더보드 시스템
- ScoreboardManager: 점수판의 데이터를 관리하며, 파일에 저장 및 로드하는 기능을 제공
- ScoreEntry: 각 점수 항목을 나타내는 데이터 클래스
- Scoreboard: 여러 점수 항목을 포함하는 리스트를 관리하는 데이터 클래스
</br>

![image](https://github.com/Team-TRY/Ansan1986/assets/111439484/54f72c2d-fcb0-48eb-8250-eb9aa7bf716c)

버스 정류장 컷씬 시스템
- IToggleStrategy: 기능 토글 동작을 정의하는 인터페이스
- 라디오, 전등, 와이퍼 등의 전략 클래스에서 인터페이스 구현
- ToggleController: 현재 세팅된 전략을 실행하는 컨트롤러 클래스
- 각 기능의 초기화 클래스들에서 특정 전략을 세팅
</br>

![image](https://github.com/Team-TRY/Ansan1986/assets/111439484/584b3b60-ab02-4317-bce6-ac8cb36a4cda)

버스 내부 기능 컨트롤 시스템
- IBusStopState: 컷씬 동작을 정의하는 인터페이스
- 각 3개 정류장의 컷씬 상태에 관한 클래스 설정
- BusCutSceneContext: 현재 상태를 관리하고 상태에 따라 동작을 부여하는 컨텍스트 클래스
- 각 기능의 초기화 클래스들에서 특정 상태를 컨텍스트 클래스에 설정
</br>

## 개발 스택
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-007ACC?style=for-the-badge&logo=Visual%20Studio&logoColor=white)</br>
![Unity](https://img.shields.io/badge/Unity-ffffff?style=for-the-badge&logo=Unity&logoColor=black)</br>
![Git](https://img.shields.io/badge/Git-F05032?style=for-the-badge&logo=Git&logoColor=white)</br>
![Github](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=GitHub&logoColor=white)           
![CSharp](https://img.shields.io/badge/CSharp-8977AD?style=for-the-badge&logo=CSharp&logoColor=white)


 
