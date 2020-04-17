# Unity 开发代码风格

Based on https://github.com/raywenderlich/c-sharp-style-guide

- [Unity 开发代码风格](#unity-%e5%bc%80%e5%8f%91%e4%bb%a3%e7%a0%81%e9%a3%8e%e6%a0%bc)
  - [A.命名法则](#a%e5%91%bd%e5%90%8d%e6%b3%95%e5%88%99)
    - [A.1 Namespaces](#a1-namespaces)
    - [A.2 Classes](#a2-classes)
    - [A.3 Methods](#a3-methods)
    - [A.4 Fields](#a4-fields)
    - [A.5 参数](#a5-%e5%8f%82%e6%95%b0)
    - [A.6 Actions](#a6-actions)
    - [A.7 Misc](#a7-misc)
  - [B. 声明](#b-%e5%a3%b0%e6%98%8e)
    - [B.1 修饰词](#b1-%e4%bf%ae%e9%a5%b0%e8%af%8d)
    - [B.2 Fields & Variables](#b2-fields--variables)
  - [C. 空格](#c-%e7%a9%ba%e6%a0%bc)
  - [D. 大括号](#d-%e5%a4%a7%e6%8b%ac%e5%8f%b7)
  - [E. 注释](#e-%e6%b3%a8%e9%87%8a)
  - [F. Region](#f-region)

## A.命名法则

### A.1 Namespaces

- 每个脚本文件都应该属于一个 namespace，我们项目的所有脚本文件应该位于一个统一的 namespace 里。

- Example:

  ```c#
  namespace Com.MyCompany.MyGame
  {
      [.....]
  }
  ```

- Group 1 使用 `Com.GlassBlade.Group1`

- Group 2 使用 `Com.GlassBlade.Group2`

- Back 使用 `Com.GlassBlade.Back` 



### A.2 Classes

- 类的命名一律驼峰式(__PascalCase__)，例如 `public class GameManager`



### A.3 Methods

- 方法的命名一律驼峰式，例如 `DoSomething()`



### A.4 Fields

- 采用首字母小写的驼峰式(__camelCase__), 例如:

  ```c#
  public class MyClass
  {
      public int publicField;
      int packagePrivate;
      private int myPrivate;
      protected int myProtected;
  }
  ```

- 尽量避免下划线开头的命名

- static 的 Field 应使用 __PascalCase__

  ```c#
  public static int TheAnswer = 42;
  ```

  

### A.5 参数

- 采用首字母小写的驼峰式 (__camelCase__), 例如：

  ```c#
  void DoSomething(Vector3 location)
  ```



### A.6 Actions

- 采用 __PascalCase__: `public event Action<int> ValueChanged`



### A.7 Misc

- 我们认为缩写是一个 word, 例如，避免 `findPostByID` 而应使用 `findPostById`



## B. 声明



### B.1 修饰词

- 声明的前置修饰词 (`public` `private`....) 应该被明确写出，不能省略



### B.2 Fields & Variables

- 每行一个声明

   __AVOID__: `string username, twitterHandle`

   __PREFER:__

    ```c#
string username;
string twitterHandle;
    ```



## C. 空格

- 使用空格而不是 tab

  - 一般编辑器都会把 tab 转换成空格

- 使用 __4个空格__ 的缩进

- LineWrapper : 4个空格 (待商议)

  __AVOID:__

  ```c#
  CoolUiWidget widget =
          someIncrediblyLongExpression(that, reallyWouldNotFit, on, aSingle, line);
  ```

  __PREFER:__

  ```c#
  CoolUiWidget widget =
      someIncrediblyLongExpression(that, reallyWouldNotFit, on, aSingle, line);
  ```

- 一行代码长度不超过 80 character （待商议）

- 每个方法之间有 __1行__ 空白行

- if 等语句后应该有一个空格 `if (someTest) ....`

- 避免行尾空格

  



## D. 大括号

- 使用 C# 的风格

  __AVOID__

  ```c#
  class MyClass {
      public void DoSomething() {
          if (someTest) {
              // ....
          } else {
              // .....
          }
      }
  }
  ```

  __PREFER:__

  ```c#
  class MyClass
  {
      void DoSomething()
      {
          if (someTest)
          {
              //...
          }
          else
          {
              //...
          }
      }
  }
  ```

- 哪怕 if 只有 1 行也应有大括号

- switch statement 仅在需要的时候加入



## E. 注释

- 避免代码后的注释 (待商议)

  __AVOID:__

  ```c#
  int clock;	// global clock
  ```

  __PREFER:__

  ```c#
  // global clock
  int clock;
  ```

- 尽量为函数写上文档：

  ```c#
  /// <summary>
  /// docs here
  /// </summary>
  public void DoSomething()
  {
      // ...
  }
  ```



## F. Region

（仅后端）我们将每个声明/定义都放在一个特定的 region 中，例如：

```c#
using UnityEngine;
using System.Collections;

namespace Com.MyCompany.MyGame
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private float directionDampTime = .25f;
        private Animator animator;

        #endregion

        #region MonoBehaviour CallBacks

        // Use this for initialization
        void Start()
        {
			// ...
        }

        // Update is called once per frame
        void Update()
        {
            // ...
        }

        #endregion
    }
}
```





--------------------



__注:__ 在 Visual Studio 中可以通过快捷键 `Ctrl + E` `Ctrl + D` 快速 format 代码 [[ref](https://stackoverflow.com/questions/4942113/is-there-a-format-code-shortcut-for-visual-studio)]