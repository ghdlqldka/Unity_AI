# Unity AI - 모방학습 예제 프로젝트

Unity ML-Agents를 활용한 모방학습(Imitation Learning) 구현 예제 프로젝트입니다.

## 📋 프로젝트 소개

이 프로젝트는 Unity ML-Agents Toolkit을 사용하여 AI 에이전트가 인간의 행동을 모방하여 학습하는 모방학습을 구현한 예제입니다. 강화학습과 달리 보상 함수 없이도 전문가의 시연 데이터를 통해 AI가 학습할 수 있는 방법을 보여줍니다.

## 🚀 주요 기능

- **모방학습 구현**: 전문가 시연 데이터를 활용한 AI 학습
- **Unity ML-Agents 통합**: Unity 환경에서 AI 에이전트 훈련
- **실시간 학습 모니터링**: 학습 과정 시각화 및 성능 추적
- **다양한 환경 지원**: 2D/3D 게임 환경에서의 AI 적용

## 🛠️ 기술 스택

- **게임 엔진**: Unity 2020.3 LTS 이상
- **AI 프레임워크**: Unity ML-Agents Toolkit
- **프로그래밍 언어**: C#, Python
- **머신러닝**: PyTorch 기반 신경망

## 📦 설치 및 설정

### 1. Unity 환경 설정

```bash
# Unity Hub를 통해 Unity 2020.3 LTS 이상 설치
# ML-Agents Package Manager에서 설치:
# Window > Package Manager > + > Add package from git URL
# com.unity.ml-agents
```

### 2. Python 환경 설정

```bash
# Python 3.7-3.9 설치 권장
pip install mlagents
pip install torch
pip install numpy
```

### 3. 프로젝트 클론

```bash
git clone https://github.com/ghdlqldka/Unity_AI.git
cd Unity_AI
```

## 🎮 사용 방법

### 1. Unity에서 프로젝트 열기

1. Unity Hub에서 "Add" 클릭
2. 클론한 프로젝트 폴더 선택
3. Unity 버전 확인 후 프로젝트 열기

### 2. 시연 데이터 수집

```bash
# Unity에서 시연 모드로 실행
# 키보드 조작으로 에이전트 직접 제어
# 시연 데이터가 자동으로 .demo 파일로 저장됨
```

### 3. 모방학습 훈련

```bash
# 명령 프롬프트에서 실행
mlagents-learn config/trainer_config.yaml --run-id=imitation_run --resume
```

### 4. 학습 결과 확인

```bash
# TensorBoard로 학습 과정 모니터링
tensorboard --logdir results
```

## 📁 프로젝트 구조

```
Unity_AI/
├── Assets/
│   ├── ML-Agents/          # ML-Agents 관련 스크립트
│   ├── Scripts/            # 게임 로직 스크립트
│   ├── Scenes/             # Unity 씬 파일
│   ├── Materials/          # 머티리얼 및 텍스처
│   └── Prefabs/            # 프리팹 오브젝트
├── config/                 # ML-Agents 설정 파일
├── results/                # 학습 결과 저장
└── demonstrations/         # 시연 데이터 저장
```

## 🤖 에이전트 학습 과정

1. **데이터 수집**: 인간 플레이어의 행동 데이터 수집
2. **전처리**: 시연 데이터를 학습 가능한 형태로 변환
3. **신경망 훈련**: 행동 복제(Behavioral Cloning) 알고리즘 적용
4. **성능 평가**: 학습된 모델의 성능 측정 및 개선

## ⚙️ 설정 파일

### trainer_config.yaml 예시

```yaml
behaviors:
  YourAgentBehavior:
    trainer_type: bc
    hyperparameters:
      learning_rate: 3.0e-4
      batch_size: 1024
      buffer_size: 10240
    behavioral_cloning:
      demo_path: demonstrations/YourDemo.demo
      strength: 1.0
      steps: 50000
```

## 📊 학습 모니터링

학습 진행 상황은 TensorBoard를 통해 실시간으로 확인할 수 있습니다:

- **Loss**: 손실 함수 값의 변화
- **Reward**: 에이전트의 성능 지표
- **Episode Length**: 에피소드 길이 통계

## 🔧 문제해결

### 일반적인 문제들

**Q: ML-Agents 패키지를 찾을 수 없습니다**
```bash
# Package Manager에서 Unity Registry 확인
# 또는 Git URL로 직접 설치: com.unity.ml-agents
```

**Q: Python 환경 오류**
```bash
# 가상환경 생성 권장
python -m venv ml-agents-env
source ml-agents-env/bin/activate  # Windows: ml-agents-env\Scripts\activate
pip install mlagents
```

**Q: 학습이 진행되지 않습니다**
- 시연 데이터 경로 확인
- config 파일의 behavior 이름 일치 여부 확인
- Unity Editor가 Play 모드에서 실행 중인지 확인

## 🤝 기여하기

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 라이선스

이 프로젝트는 MIT License 하에 배포됩니다. 자세한 내용은 `LICENSE` 파일을 참조하세요.

## 📞 연락처

프로젝트 관련 문의사항이나 제안사항이 있으시면 언제든지 연락주세요!

- GitHub: [@ghdlqldka](https://github.com/ghdlqldka)
- 프로젝트 링크: [https://github.com/ghdlqldka/Unity_AI](https://github.com/ghdlqldka/Unity_AI)

## 🙏 감사의 말

- [Unity ML-Agents](https://github.com/Unity-Technologies/ml-agents) - 훌륭한 ML 프레임워크 제공
- Unity Technologies - 강력한 게임 엔진
- PyTorch 커뮤니티 - 딥러닝 라이브러리

---

⭐ 이 프로젝트가 도움이 되셨다면 Star를 눌러주세요!
