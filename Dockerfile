FROM mono:4.0.1
RUN mkdir -p /app/build/

#COPY ServiceStack*.dll /app/src/
#COPY hello.fs /app/src/hello.fs
#RUN fsharpc --lib:. -r:ServiceStack.Common.dll -r:ServiceStack.Interfaces.dll -r:ServiceStack.Text.dll -r:ServiceStack.Client.dll -r:ServiceStack.dll hello.fs

#COPY . /app/
# WORKDIR /app/
# RUN mono nuget.exe install FAKE -ExcludeVersion -OutputDirectory lib -version 4.0.1
# RUN mono nuget.exe install ServiceStack -ExcludeVersion -OutputDirectory lib -Version 4.0.42
# RUN mono "lib/FAKE/tools/FAKE.exe" build.fsx BuildApp

COPY build/ /app/build/
COPY scowl_word_lists/ /app/scowl_word_lists/

WORKDIR /app/
CMD [ "mono",  "build/WordSearch.exe" ]
