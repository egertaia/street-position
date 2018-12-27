# Street Positions NFive Plugin
[![License](https://img.shields.io/github/license/egertaia/street-position.svg)](LICENSE)
[![Build Status](https://img.shields.io/appveyor/ci/egertaia/street-position.svg)](https://ci.appveyor.com/project/egertaia/street-position)
[![Release Version](https://img.shields.io/github/release/egertaia/street-position/all.svg)](https://github.com/egertaia/street-position/releases)

This plugin adds a customizable HUD element to display current street location, area and direction to your [NFive](https://github.com/NFive) [FiveM](https://fivem.net/) GTAV server.

This project aims to replace GTAV's native street and area display with a fully configurable one, that by default is positioned next to the minimap.

![Screenshot](https://user-images.githubusercontent.com/9960794/50387612-3c57d500-0709-11e9-865d-31d6ffe76f9a.png)

## Installation
Install the plugin into your server from the [NFive Hub](https://hub.nfive.io/egertaia/street-position): `nfpm install egertaia/street-position`

## Configuration
```yml
# When to display the HUD element
when:
  on_foot: true
  in_vehicle: true

# Which HUD elements to display
display:
  street: true    # The current street name
  crossing: true  # The closest street crossing
  area: true      # The name of the map area
  direction: true # The compass direction you are facing

# How often, in milliseconds, to update the location
update_interval: 500

# (Optional) The RPC event which when fired should start this plugin
# If set, this plugin won't display anything until after this event has fired once
# This can be used to enable this plugin once you are loaded into the game
# Defaults to disabled
activation_event: something:game:started

# HTML template with placeholder values, this can be edited to fully customize the HUD
# Edit "server\resources\nfive\plugins\egertaia\street-position\Overlays\style.css" to edit the styling
template: >-
  <div id="left-section">
  	<span id="direction">{ Direction }</span>
  </div>

  <div id="right-section">
  	<div id="top-row">
  		<span id="street">{ Street }</span> in <span id="area">{ Area }</span>
  	</div>
  	<div id="bottom-row">
  		Crossing <span id="crossing">{ Crossing }</span>
  	</div>
  </div>
```
