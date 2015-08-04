FROM mono:3.10
RUN mkdir -p /app/src
WORKDIR /app/src/
#COPY ServiceStack*.dll /app/src/
#COPY hello.fs /app/src/hello.fs
COPY build /app/
#RUN fsharpc --lib:. -r:ServiceStack.Common.dll -r:ServiceStack.Interfaces.dll -r:ServiceStack.Text.dll -r:ServiceStack.Client.dll -r:ServiceStack.dll hello.fs
CMD [ "mono",  "./WordSearch.exe" ]
