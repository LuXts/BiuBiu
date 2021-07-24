
# BiuBiu 一个局域网聊天IM

本项目旨在实现一个局域网通信软件

功能实现：在线聊天，图片发送，文件传递，多人聊天，用户管理（包括注册新用户，审核用户，用户信息维护等功能）

没实现的功能：退出群聊，解散群聊，视频聊天（只实现了按钮）

## 项目结构

### `BiuBiuAdminWpfClient`
 
项目的管理员客户端，界面写得不好看，数据绑定没用上。很不优雅，需要在 `Initialization.cs` 配置服务器位置才能跑起来。

### `BiuBiuServer` 

项目的服务器部分，需要在 `Initialization.cs` `MySqlDriven` 配置位置才能跑起来。

#### 文件夹

`Authentication` 登录验证相关

`Database` 数据库驱动

`Interfaces` 数据库驱动的接口

`OnlineHub` 在线状态实现，客户端需要上线后连接这个部分查询在线列表（也防止客户端重复登录）

`Services` 服务的具体实现

`TeamHub` 群组聊天实现

`Tests` 没有用的测试类，不参与使用

`Userhub` 用户在线聊天实现

``

### `BiuBiuShare` 

项目的分享文件，里面放着一部分共用的工具类和服务器客户端依赖的接口。

### `BiuBiuShareTerminalClient` 

测试用的终端客户端，基本上乱写的

### `BiuBiuWpfClient` 
项目的聊天客户端，主界面塞了太多逻辑，非常混乱。