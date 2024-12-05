# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [unreleased] - 2024-12-xx

### Added

- Made settings revert if apply was never pressed.
- OnCloseSettings callback.
- OnCancelSettings callback.
- Button elements.

### Fixed

- Excess logs when rebinding a key.
- Dropdowns not initializing their value.
- ApplySettings now gets called before the settings close.

## [1.0.0] - 2024-12-03

### Added

- Mod settings tab to the settings UI.
- Class for settings UI callbacks and adding UI.
- Space, Header, Toggle, Simple Slider, Advanced Slider, Dropdown, and Key Button base elements.

[unreleased]: https://github.com/Nestorboy/ATLYSS-EasySettings/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/Nestorboy/ATLYSS-EasySettings/releases/tag/v1.0.0