# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.1] - 2025-11-22

### Fixed

- Issues caused by game version 112025.a1
- Text settings truncating initial values longer than 32 characters

## [1.2.0] - 2025-10-04

### Added

- Text field elements, thanks to FleeTime!

## [1.1.8] - 2025-07-12

### Fixed

- Key button being used as the regular button.

## [1.1.7] - 2025-07-07

### Fixed

- Error when initializing toggle template.

## [1.1.6] - 2025-05-04

### Fixed

- Error when resetting an int slider.

## [1.1.5] - 2025-05-03

### Fixed

- Int range not being used for the advanced slider.

## [1.1.4] - 2025-05-02

### Fixed

- Dropdown index being treated as the enum value, causing mismatches when using custom enum values.

## [1.1.3] - 2024-12-07

### Fixed

- OnValueChanged not being called for Toggles and Simple Sliders.

## [1.1.2] - 2024-12-07

### Changed

- Made the mod only work with ATLYSS.exe.

### Fixed

- Fixed advanced slider value displays not initializing properly under specific conditions.

## [1.1.1] - 2024-12-05

### Fixed

- Temporary test elements appearing in the settings.

## [1.1.0] - 2024-12-05

### Added

- Made settings revert if apply was never pressed.
- OnCloseSettings callback.
- OnCancelSettings callback.
- Button elements.

### Fixed

- Excess logs when rebinding a key.
- Dropdowns not initializing their value.
- OnApplySettings now gets called before the settings close.

## [1.0.0] - 2024-12-03

### Added

- Mod settings tab to the settings UI.
- Class for settings UI callbacks and adding UI.
- Space, Header, Toggle, Simple Slider, Advanced Slider, Dropdown, and Key Button base elements.

[unreleased]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.2.0...HEAD
[1.2.0]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.8...v1.2.0
[1.1.8]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.7...v1.1.8
[1.1.7]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.6...v1.1.7
[1.1.6]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.5...v1.1.6
[1.1.5]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.4...v1.1.5
[1.1.4]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.3...v1.1.4
[1.1.3]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.2...v1.1.3
[1.1.2]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.1...v1.1.2
[1.1.1]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/Nestorboy/ATLYSS-EasySettings/releases/tag/v1.0.0