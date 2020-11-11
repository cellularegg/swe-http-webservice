# Time Tracking
| Date  | Time in h | Comment |
| ------------- | ------------- | ------------- |
| 20-oct-2020 | 2.75 | Started developing HTTP Server |
| 21-oct-2020 | 3 | Added MessageCollection + Tests |
| 11-nov-2020 | 2 | Further developed MsgColl + Implemented GET /messages and POST /messages |


# REST/HTTP-based plain-text Webservices

This project is intended to create a webservice handler based on a restful HTTP-based
server. To test the general purpose implementation a message resource is created with
all CRUD-methods.

## PreConditions

To understand REST-APIs in depth please use the webpage [restfulapi.net](https://restfulapi.net/) and read the
guidelines:

- [What is REST?](https://restfulapi.net/)
- [REST Constraints](https://restfulapi.net/rest-architectural-constraints/)
- [REST Resource Naming Guide](https://restfulapi.net/resource-naming/)
- [HTTP Methods](https://restfulapi.net/http-methods/)
- [Status Codes](https://restfulapi.net/http-status-codes/)

Furthermore: [additional material to HTTP on mdn](https://developer.mozilla.org/en-US/docs/Web/HTTP/Overview)

## Task

Create a console application with TCP-based network handling in Java or C# that
implements the HTTP protocol. In both cases you are not allowed to use more than basic
TCP classes. This means for C# the usage of [TcpListener](https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=netcore-3.1) and for Java the usage of
[SocketServer](https://docs.oracle.com/javase/7/docs/api/java/net/ServerSocket.html) (see: [here](https://docs.oracle.com/javase/tutorial/networking/sockets/clientServer.html)).

Implement the HTTP format, so that you

- can read the HTTP-Verb, the resource requested and the http-version
- can read the further header values and manage it as a key-value pair
- can read the payload correctly as plaintext (text/plain MIME-Type).

Create a class containing these values called RequestContext and make it available to
the endpoint-handlers.

Register the following Message-API-Endpoints with corresponding implementation:

```
lists all messages: GET /messages
add message: POST /messages (Payload: the message; Response an id like
1)
show first message: GET /messages/1
show third message: GET /messages/3
update first message: PUT /messages/1 (Payload: the message)
remove first message: DELETE /messages/1
```

Log all information that is retrieved by an HTTP request clearly, well-arranged and
understandable to the console window.

## Tests

Test the webservice handler to verify that it works. Create automated unit and
integration tests.
For the integration tests you should:
- test manually using the browser and
- with tools like
  - curl (pre-installed on win10 or linux),
  - [Postman](https://www.postman.com/)
  - [Insomnia](https://insomnia.rest/)
(Curl recommended, because it is easy to script the tests)

## HandIn

- hand in via moodle (check the due date in moodle)
- pay attention on the patterns corresponding to restful APIs, code quality and
  <hint>reusability</hint>
- mandatory: create a 5-7 min video (standard VLC-playable format) with
  - demonstration that all tests (automated, manual, browser) work
  - rough explanation of the code (favored with class diagram)
  - pitfalls
- penalty for late submissions: 0,5 points per day

