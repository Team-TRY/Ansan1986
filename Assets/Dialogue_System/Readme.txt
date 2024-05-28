Dialogue System Usage Manual

Step 1: Setting Up the Dialogue Data

Create Dialogue Data ScriptableObject:

In the Unity Editor, right-click in the Assets folder (or any preferred folder).
Select Create -> ScriptableObjects -> DialogueData.
This will create a new DialogueDataScriptableObject instance.
Edit Dialogue Data:

Click on the newly created DialogueDataScriptableObject instance.
In the Inspector, you will see a Dialogues list.
Click the + button to add new dialogues.
For each dialogue, enter the Content (the text of the dialogue) and Display Time (the time in seconds the dialogue will be displayed).

Step 2: Assigning the Dialogue Data to the Dialogue Manager
Attach DialogueManager Script:

Create an empty GameObject in your scene (e.g., name it DialogueManager).
Attach the DialogueManager script to this GameObject by dragging the script onto the GameObject in the Inspector.
Assign TextMeshProUGUI Component:

Ensure you have a TextMeshPro - Text component in your UI to display the dialogues.
Assign this TextMeshProUGUI component to the Dialogue Text field of the DialogueManager script in the Inspector.
Assign DialogueData ScriptableObject:

Drag the DialogueDataScriptableObject instance you created earlier into the Dialogue Data field of the DialogueManager script in the Inspector.

Step 3: Running the Dialogue System
Start the Scene:

Ensure your scene contains the GameObject with the DialogueManager script and a TextMeshProUGUI component properly assigned.
Press the Play button in the Unity Editor to start the scene.
Observe the Dialogues:

The dialogues will appear on the screen in sequence according to the content and display time specified in the DialogueDataScriptableObject.

--

대화 시스템 사용 매뉴얼
1단계: 대화 데이터 설정
Dialogue Data ScriptableObject 생성:

Unity 에디터에서 Assets 폴더(또는 원하는 폴더)에서 오른쪽 클릭합니다.
Create -> ScriptableObjects -> DialogueData를 선택합니다.
새로운 DialogueDataScriptableObject 인스턴스가 생성됩니다.
대화 데이터 편집:

새로 생성된 DialogueDataScriptableObject 인스턴스를 클릭합니다.
Inspector 창에서 Dialogues 리스트를 확인할 수 있습니다.
+ 버튼을 클릭하여 새로운 대화를 추가합니다.
각 대화에 대해 Content(대화 텍스트)와 Display Time(대화가 표시될 시간(초))을 입력합니다.
2단계: 대화 데이터를 Dialogue Manager에 할당
DialogueManager 스크립트 부착:

씬(Scene)에서 빈 GameObject를 생성합니다(예: 이름을 DialogueManager로 설정).
Inspector 창에서 GameObject에 DialogueManager 스크립트를 드래그하여 부착합니다.
TextMeshProUGUI 컴포넌트 할당:

대화를 표시할 TextMeshPro - Text UI 컴포넌트가 있는지 확인합니다.
Inspector 창에서 DialogueManager 스크립트의 Dialogue Text 필드에 이 TextMeshProUGUI 컴포넌트를 할당합니다.
DialogueData ScriptableObject 할당:

Inspector 창에서 DialogueManager 스크립트의 Dialogue Data 필드에 이전에 생성한 DialogueDataScriptableObject 인스턴스를 드래그하여 할당합니다.
3단계: 대화 시스템 실행
씬 시작:

씬에 DialogueManager 스크립트가 부착된 GameObject와 적절히 할당된 TextMeshProUGUI 컴포넌트가 있는지 확인합니다.
Unity 에디터에서 Play 버튼을 눌러 씬을 시작합니다.
대화 확인:

지정된 DialogueDataScriptableObject에 정의된 내용과 표시 시간에 따라 대화가 순차적으로 화면에 표시됩니다.
요약 예시
데이터 생성 및 편집:

DialogueDataScriptableObject를 생성합니다.
텍스트와 표시 시간을 포함한 대화 항목을 추가합니다.
컴포넌트 설정:

DialogueManager 스크립트를 GameObject에 부착합니다.
스크립트에 TextMeshProUGUI 컴포넌트를 할당합니다.
DialogueDataScriptableObject를 스크립트에 할당합니다.
실행 및 테스트:

재생 버튼을 눌러 대화가 화면에 표시되는지 확인합니다.