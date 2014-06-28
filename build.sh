#!/bin/sh
# Type './build.sh' to generate C# classes.

if [ ! -f ../vgcore/win/vc2010.sln ] ; then
    git clone https://github.com/touchvg/vgcore ../vgcore
fi
if [ ! -f ../vgwpf/Test_cs10.sln ] ; then
    git clone https://github.com/touchvg/vgwpf ../vgwpf
fi
if [ ! -f ../DemoCmds/wpf/vs2010.sln ] ; then
    git clone https://github.com/touchvg/DemoCmds ../DemoCmds
fi
cd ../vgwpf; sh build.sh $1; cd ../vgwpf-demo
cd ../DemoCmds/wpf; sh build.sh $1; cd ../../vgwpf-demo
