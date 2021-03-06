# UguiHole

## 介绍

在游戏中新手引导等需求的时候我们经常需要进行挖洞操作。即在一个遮罩区域显示某个UI元素指引玩家进行点击。

支持打了图集的sprite。

## 设计思路

一个比较简单的方案就是利用Unity的反向Mask。即利用模板缓冲来实现，这里给出一个参考过的网上的文章

https://www.cnblogs.com/j349900963/p/8340571.html

这种挖洞操作的好处就是，挂载了脚本都可以直接进行挖洞，各个hole在模板缓冲区进行交流。

另一种方式就是使用shader去挖洞，这里采用的是这一种方案。通过传入要挖洞区域的位置信息以及texture信息，在渲染遮罩的时候对应位置渲染成透明即可。

这样会有一个问题就是，不知道要挖多少个洞，如果要挖多个洞的话就要传入多个数据。

图集支持:直接获取sprite在图集里的uv，然后传入shader即可。

## 效果展示

![img](https://github.com/jechyang/UguiHole/blob/main/ReadMeImg/hole.gif)

## 使用方式

* 在遮罩图片上挂载**HoleMgr**组件。
* 在需要使用的地方获取HoleMgr，并且调用**SetHoles(List<Images>)**传入要加遮罩的Image Component的List。
* 调用HoleMgr的**RefresHole()**方法。

详情可以查看项目中的TestScript脚本。

## 挖洞数量

目前默认支持挖四个洞，如果需要增加挖洞数量的话需要按照以下步骤进行更改。

1. 修改HoleMgr里的**_maxHoleCount**变量。

   ![img](https://github.com/jechyang/UguiHole/blob/main/ReadMeImg/addVar.png)

2. 在**HoleShader**里按照格式增加变量。

   ![img](https://github.com/jechyang/UguiHole/blob/main/ReadMeImg/addShaderVar.png)

3. 在frag函数里调用**getColor**函数获取对应texture的颜色。

   ![img](https://github.com/jechyang/UguiHole/blob/main/ReadMeImg/modifyFrag.png)



