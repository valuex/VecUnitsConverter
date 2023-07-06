# VecUnitsConverter
A simple and customizable Unit Converter

# Features
1. User can define his/her own units, he/she can do setting in the `UnitsConfig.json` file
2. One can start converting by just input the unit name in the Inputbox, the corresponding units will be display after press Enter or Click the `GO!` button
   - input just unit name (case unsensitive), like `km`, the  `Length` related units will be displayed, and the textbox with label name as `km` will get the focus;
   - input `Value + UnitName` (case unsensitive), like `1km` or `1 km` (space between `Value` and `UnitName` is optional),the  `Length` related units will be displayed, and the textbox with label name as `km` will get the focus; and the converting will be carried out automatically.
3. Double-click the textbox to copy the value
4. Click the Unit Label, the converted value and Unit label will be copied at the same time
5. Can accept arguments. All arguments will be merged into one single string seperated by space.
#  About `UnitsConfig.json`
*  one and only one `basic unit` shall be defined under the `UnitTypeName`

```json



{
        "UnitTypeName": "Area",
        "Units": [
            {
                "UnitFullName": "Meter^2",
                "UnitShortName": "m^2",
                "IsBasicUnit": true,
                "EqualsToBasicUnit": 1
                "IsFavorite":true

            },
            {
                "UnitFullName": "MiliMeter^2",
                "UnitShortName": "mm^2",
                "IsBasicUnit": false,
                "EqualsToBasicUnit": 1E-6
            },
            {
                "UnitFullName": "KiloMeter^2",
                "UnitShortName": "Km^2",
                "IsBasicUnit": false,
                "EqualsToBasicUnit": 1E6
            }
        ]
    }
```
