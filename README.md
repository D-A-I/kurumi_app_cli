# kurumi_app_cli

1. GetAll のテスト  
-> OK
1. GetByGroupId のテスト 　
-> OK
1. log4net の導入
1. moq.. 適用対象を考える  
※ Test Explorerとコードカバレッジは、VS for macで確認する
> 参考）テストのショートカット  
- tasks.json
```
 {
     "label": "run tests",
     "type": "shell",
     "command": "dotnet test"
 }
```
- keybindings.json
```
{
    "key": "cmd+'",
    "command": "workbench.action.tasks.runTask",
    "args": "run tests",
    "when": "editorTextFocus"
}
```