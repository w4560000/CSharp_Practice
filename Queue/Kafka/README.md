## �Ұʮe��
```
docker-compose up -d
```

## �޲z UI
http://localhost:8080/

## ����
Topic = �D�D
Partition = ����


1 �� Topic �� ���h�� Partition


Consumer ���O ���w topic�BGroupId

enable.auto.commit = �۰ʴ���ثe���O��offset
auto.commit.interval.ms = (default 5000 ms ) enable.auto.commit �� true �ɡA�۰ʴ��檺���j�ɶ�
max.poll.records = (default 500 ) �榸���O��������
max.poll.interval.ms = (default 30000 ms)�Y�b�ɶ����S�����O�� poll �������A�h�������O���ѡAbroker�|�N��consumer�����AĲ�orebalance�Aconsumer���s�[�Jgroup�A���s���O

## �ѦҤ峹
https://dimosr.github.io/kafka-docker/
https://www.readfog.com/a/1635090175644241920
https://zhuanlan.zhihu.com/p/112745985