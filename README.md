# Naruto.WebSocket

#### 介绍
此项目是使用中间件对WebSocket 进行封装，方便使用
<br>单机10000连接，内存占用220M
#### 软件架构
软件架构采用的是.Net Core 6.0
#### 支持
1. 支持点对点的发送消息，支持群聊消息，支持发送所有人的消息，支持发送指定用户消息，支持集群扩展,支持多租户的模式.
2. 支持授权验证
3. 用户可以使用<b>NarutoWebSocketEvent</b>处理上下线的事件通知
4. 集群版默认使用的redis的发布订阅功能，消息可能存在丢失（后续计划实现ack）
5. 目前仅支持文本发送，后续计划更新MessagePack消息协议

#### 使用说明
1. 核心对象<b>NarutoWebSocketService</b>，处理接收服务的操作,使用者需要继承此对象，实现自己的方法，且方法的访问级别为Public,方法的参数支持无参和实例对象两种，并且方法的返回值必须为Task,并且原生支持DI，生命周期为作用域Scope模式
2. 示例 安装Nuget包<b>Naruto.WebSocket</b>, 并注入所需服务和对应的接收服务的对象
```c#
            //注入服务
            services.AddNarutoWebSocket<MyService>(a =>
            {
                a.Path = new PathString("/ws");//websocket的请求路径
                a.AuthorizationFilters.Add(new MyAuthorizationFilters());//追加websocket连接的授权信息
            });
```
3. 服务端也可以使用<b>IClientSend\<TService></b>操作消息的发送
4. 集群版,安装Nuget包<b>Naruto.WebSocket.Redis</b>,然后还需注入
```c#
            //注入集群版需要的服务
            services.AddNarutoWebSocketRedis(a => a.Connection = new string[] { "127.0.0.1:6379" });


```
5. 消息的发送和接收的都是采用标准的<b>json</b>格式
6. 客户端发送接收的消息格式
``` javascript
     var msg =  {
            action: "send",//调用的后端/前端的方法，大小写必须一致
            message: object//发送的消息内容 消息内容为json对象格式
        }
        webSocket.send(JSON.stringify(msg));
```
7. 客户端当发送的action为<b>HeartbeatCheck</b>,代表执行的心跳检查，客户端应每隔60s执行一次心跳检查
8. 当执行的action，无法在后端找到对应的方法的时候，连接将会断开
9. 用户可以在创建websocket客户端的时候主动传递一个当前websocket的连接Id，不传递则由后台自动生成
```javascript
    //主动传递一个连接Id的值ConnectionId
     var webSocket = new WebSocket("ws://localhost:5003/ws?ConnectionId=12345678");
```
#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request