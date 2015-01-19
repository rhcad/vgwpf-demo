# TouchVG Demo for WPF

This is a unit test and example project for [vgwpf](https://github.com/rhcad/vgwpf), which is a lightweight 2D vector drawing framework for Windows (WPF).

![arch](http://touchvg.github.io/images/arch.svg)

## License

This is an open source [LGPL 2.1](LICENSE.md) licensed project. It uses the following open source projects:

- [vgwpf](https://github.com/rhcad/vgwpf) (LGPL): Vector drawing framework for Windows.
- [vgcore](https://github.com/rhcad/vgcore) (LGPL): Cross-platform vector drawing libraries using C++.
- [DemoCmds](https://github.com/rhcad/DemoCmds): A template and example project containing customized shape and command classes.

## How to Compile

- Open `Test_cs10.sln` in Visual Studio 2010 (Need VC++ and C#), then run the`WpfDemo` application. Or open `Test_cs9.sln` in VS2008.

  - Need to install the lastest version of [SWIG](http://sourceforge.net/projects/swig/files/), and add the location to PATH.
  
- Type `./build.sh` can regenerate `touchvglib/core/*.cs`.
 
## Add more shapes and commands

- Do not want to write C++ code? Please reference to [testview/shape](https://github.com/rhcad/vgandroid-demo/tree/master/test/src/vgtest/testview/shape) package to write your own shape and command classes.

- You can create library project containing your own shapes and commands. So the TouchVG and TouchVGCore libraries does not require changes.

  - Checkout and enter [DemoCmds](https://github.com/rhcad/DemoCmds) directory, then type `python newproj.py YourCmds`:

     ```shell
     git clone https://github.com/rhcad/DemoCmds.git
     cd DemoCmds
     python newproj.py MyCmds
     ```

- You can customize the drawing behavior via implement your CmdObserver class (see the example in [DemoCmds](https://github.com/rhcad/DemoCmds) ).

## How to Contribute

Contributors and sponsors are welcome. You may translate, commit issues or pull requests on this Github site.
To contribute, please follow the branching model outlined here: [A successful Git branching model](http://nvie.com/posts/a-successful-git-branching-model/).
