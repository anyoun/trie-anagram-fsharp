FROM python:2.7.9

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY requirements.txt /usr/src/app/
RUN pip install -r requirements.txt

#COPY main.py /usr/src/app/
#COPY slouchex /usr/src/app/slouchex/
VOLUME /usr/src/app
#CMD [ "python", "/usr/src/app/main.py" ]

