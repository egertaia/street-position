# Street positions
[![License](https://img.shields.io/github/license/NFive/NFive.svg)](LICENSE)
[![Build Status](https://img.shields.io/appveyor/ci/NFive/nfive.svg)](https://ci.appveyor.com/project/egertaia/street-position)
[![Release Version](https://img.shields.io/github/release/NFive/NFive/all.svg)](https://github.com/egertaia/street-position/releases)

## What is this?
This is a plugin that works with [NFive](https://github.com/NFive/NFive) which is complete plugin framework for GTAV [FiveM](https://fivem.net/).
The whole server is built and managed entirely in C#.
This project aims to replace GTAV's native street and area showing with a fully configurable one, that by default is positioned next to the map.
![image](https://user-images.githubusercontent.com/9960794/50387612-3c57d500-0709-11e9-865d-31d6ffe76f9a.png)

### Usage
1. Make sure you are using [nfpm](https://github.com/NFive/nfpm) installed.

2. Make sure you have your project installed in a seperate folder using `nfpm setup`.

3. Install this plugin by calling `nfpm install egertaia/street-position`

4. Configure this if you are not happy with default configuration. This can be done by config file `path-to-server\resources\nfive\config\egertaia\street-position` and style/html file `path-to-server\resources\nfive\plugins\egertaia\street-position\Overlays`

#### Configuration
* `display_on_foot` = Should position be shown on foot?
* `display_in_vehicle` = Should position be shown while in vehicle?
* `show_street` = Should street name change be taken into update consideration and should this be parsed.
* `show_crossing` = Should crossing stret name be taken into update consideration and should this be parsed.
** NB! When changing this to false, you have to change `format` section otherwise there is going to be just the word "Crossing" in the bottom row.
* `show_area` = Should area name be taken into update consideration and should this be parsed.
* `show_direction` = Should direction name be taken into update consideration and should this be parsed.
** NB! This might be the only one that setting `false` to works out of the box. Other variations are untested.
* `format` = this is the html string that is going to be sent from server to the GTAV, where NUI renders this.
** `left-section` - the container for direction
** `right-section` - the container for right section.
*** `top-row` - the row that is the 1st line of text.
*** `bottom-row` - the row that is the 2nd line of text.


