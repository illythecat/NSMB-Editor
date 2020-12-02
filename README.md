# NSMB-Editor
Forked from https://github.com/Mero-Mero/NSMB-Editor originally by https://github.com/Dirbaio/NSMB-Editor

NSMBe 5.3.1 Git, modified to work with MKDS assembly
-----------------
Download NSMBe on the GitHub Release page, older versions available at NSMBHD: https://nsmbhd.net/download/

To run NSMBe:
 - **Windows:** .NET Framework 4.7.2
 - **Linux, MacOSX, Others:** Mono

Necessary NSMBe files and directories:
 - Languages
 - Be.Windows.Forms.HexBox.dll
 - LICENSE.txt
 - NSMBe5.exe

At first bootup time:
 - You will be prompted to download "spritedata.xml", if there isn't one already present. This is a necessary file for proper operation and its download requires the file "classIDforSprite.txt".

Be.Windows.Forms.HexBox:
 - Current version is: 1.6.1

Websites:
 - http://nsmbhd.net/ - The NSMB Hacking Domain, a.k.a NSMBe Forum

Credits:
 - Treeki - Initial developer and maintainer
 - Dirbaio - Current developer and maintainer
 - Piranhaplant - Developer
 - Eloston (ELMario on The NSMB Hacking Domain) - New docs and Be.Windows.Forms.HexBox.dll source code
 - Mero-Mero - Developer
 - RicBent - Developer
 - TheGameratorT - Developer
 - Szymbar - Developer, adopted the structure to work with MKDS assembly
 - And all other contributers!

## Notes about MKDS assembly
In 2012, Gericom wrote [this post right here](https://nsmbhd.net/thread/1025-asm-hacking-project-template/?from=40#20201) that kickstarted the age of custom ASM hacks in Mario Kart DS. For the longest while people were using his old editor, called MKDS Course Modifier (now known as MKDS Utility Belt), but I was missing a few features related to ASM hacking that NSMB-Editor had, but MKDSCM didn't.

This repo aims to bring all of the features of NSMBe back to our MKDS grounds while preserving backwards compatibility with 8 years of history of MKDS hacking until this mod.

Here's a list of available labels and what labels are they equivalent to:
- `ansub` => same as in MKDSCM
- `arepl` => same as in MKDSCM
- `trepl` => same as in MKDSCM
- `btrpl` => 6 bytes, push{lr} and then blx (trepl)
- `ahook` => same as nsmbe's hook

I really didn't want to waste too much time on that xd
But hey, with this we can finally build on what we already have AND use all the features of NSMBe moving forwards in MKDS ASM hacking too!

# Previews
<p align="left">
  <img src="https://raw.githubusercontent.com/TheGameratorT/NSMB-Editor/master/NSMBe5/Git_Prevs/filebrowser.png" width="385" title="File Browser">
 <img src="https://raw.githubusercontent.com/TheGameratorT/NSMB-Editor/master/NSMBe5/Git_Prevs/leveleditor.png" width="400" title="Level Editor">
</p>
