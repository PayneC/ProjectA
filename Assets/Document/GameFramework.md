# 工具类
* AssetUtil
>* 资源加载工具
* PersistenceUtil
>* 数据持久化工具
* TimeUtil
>* 时间管理工具

# 管理类
* AudioManager
>* 背景音乐、音效相关的管理 
* EventManager
>* 游戏消息处理 
* UIManager
>* 游戏ui调度管理
>* common 通用ui 可以同时打开多个与其他ui间无关联
>* sub　必须依附于主ui　并且随主ui关闭而关闭
>* main　主ui同时只能开启一个　主ui间互斥
>* default 主ui缺省时默认开启的主ui
* LevelManager
>* 关卡管理　负责关卡的加载切换
* HotFixManager
>* 热更新管理　负责游戏热更新检测以及热更新处理逻辑

# 游戏流程
* AppMain Awake 初始化游戏
* AppMain Update 游戏主循环
```
graph TD
A[Update]-->B{HotFixCheck}
B-->|hasHotFix yes| C(HotFixUpdate)
B-->|hasHotFix not| D(DataUpdate)
D-->E(EventUpdate)
E-->F(LevelUpdate)
```
* AppMain OnDestroy 卸载游戏

# 游戏结构
* Model 
>* 数据相关 分模块管理数据处理数据的设置获取　数据变更消息广播
* View 
>* 视觉表现 场景、UI表现，监听Model数据变更，从Model获取所需数据进行表现更新 
>* 玩家输入 接受玩家输入，并转化为具体control操作
* Control
>* 操作处理 根据相关操作对Model数据进行处理，最终更新到Model
>* DataUpdate 处理本地数据更新（适用于实时需要表现或者检测的数据，如游戏提示相关逻辑。可摄者数据更新频率，来控制逻辑消耗）

# 目录结构
* Base 
>* 基础逻辑及底层工具
* Levels　
>* 关卡脚本
* UIS  
>* ui脚本
* Models
>* 数据管理脚本
* Controls
>* 操作处理脚本
* Entity
>* 游戏中用到的单体类，如角色、场景物件等
* NetWork
>* 游戏网络相关
>* 网络消息分发处理

# TODO
* 自动化资源打包流程
>* Lua文件打包
>* 资源按类型打包
* 自动化安装包打包流程
* 打包配置
* 远程打包
