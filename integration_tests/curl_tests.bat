REM Test Scripts for HTTP REST Webservice (Messages)

REM Get Hello
curl -X GET http://localhost:8080/hello

REM Get Messages should be empty
curl -X GET http://localhost:8080/messages

REM POST Message (Store new message)
curl -X POST http://localhost:8080/messages -d "{'Content': 'Hey This is my first message :)'}"

REM GET Messages
curl -X GET http://localhost:8080/messages

REM GET Message 1
curl -X GET http://localhost:8080/messages/1

REM POST Message (Store new message)
curl -X POST http://localhost:8080/messages -d "{'Content': 'Hey This is my SECOND message'}"

REM POST Message (Store new message)
curl -X POST http://localhost:8080/messages -d "{'Content': 'Hey This is my THIRD msg.'}"

REM PUT Message 1 (Update existing message)
curl -X PUT http://localhost:8080/messages/1 -d "{'Content': 'Edited my third message!'}"

REM GET Messages
curl -X GET http://localhost:8080/messages

REM DELETE Message 2
curl -X DELETE http://localhost:8080/messages/1

REM GET Messages
curl -X GET http://localhost:8080/messages

REM DELETE Message 100 Should fail
curl -X DELETE http://localhost:8080/messages/100

REM GET Message 100 Should fail
curl -X GET http://localhost:8080/message/100

REM PUT Message 100 Should fail
curl -X PUT http://localhost:8080/messages/100 -d "{'Content': 'Edited my third message!'}"



