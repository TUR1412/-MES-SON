# 大厅背景轮播（本地图片）

## 你要达成的效果

- 大厅背景使用人物图（尺寸不一也能正常铺满）
- 自动轮播（无按钮）
- 不影响前景 UI 可读性（会叠加暗色遮罩与噪点纹理）

## 图片放哪里？

程序会从 **exe 同级目录**读取：

`assets\\lobby_backgrounds\\`

例：如果你运行的是 `src\\MES.UI\\bin\\Release\\MES.UI.exe`，那么把图片放到：

`src\\MES.UI\\bin\\Release\\assets\\lobby_backgrounds\\`

支持格式：`.png` / `.jpg` / `.jpeg` / `.bmp`

## 关于“英雄联盟人物图”

我不会在仓库里内置/下载任何 Riot/LoL 的版权素材（避免版权风险）。

但代码支持你把 **你有权使用** 的图片放进上述目录，程序就会自动轮播加载。

