B1: Import asset vào game
B2: Sử dụng scene SplashPixOn làm scene đầu tiền trong build setting
B3: Tại scene SplashPixOn, chọn object LoadSceneAnim, trong đó có biến SceneStartOfGame thì điền string tên của scene trước đó đã load default (ví dụ Splash, Lobby,...) 
B4: Game chạy từ SplashPixOn hoặc scene gốc trước đó đều được, nếu chạy từ SplashPixOn sẽ có thêm anim splash
BONUS: Nếu build ở account nào có bản quyền có thể bỏ luôn splash của unity cho đẹp, không có thì cứ giữ logo của unity không cần thêm logo công ty

*WARNING
- Đối với ae loading scene theo index, khi thêm scene splash này thì phải thay đổi index của các scene khác
- Đối với ad loading scene theo name thì không sao

*Những pack cần thiết:
- Tween
- Odin (chủ yếu là editor, có thể bỏ đi)