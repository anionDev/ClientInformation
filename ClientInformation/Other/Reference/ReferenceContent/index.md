# ClientInformation-reference

## Overview

`ClientInformation` is a containerized service which basically runs [ClientInformationBackend](https://github.com/anionDev/ClientInformation/tree/main/ClientInformationBackend) in it.

This codeunit is implemented as usual `Dockerfile`.

## Articles

- [API-Usage](./Articles/APIUsage.md)

## Examples

To see an example of how to self-host `ClientInformation` see the [example-docker-compose-file](Examples/MinimalDockerComposeFile/docker-compose.yml).

You can also running it on your own locally by running:

```sh
git clone https://github.com/anionDev/ClientInformation.git
cd ClientInformation
scbuildcodeunits
cd ClientInformation/Other/Reference/ReferenceContent/Examples/MinimalDockerComposeFile
python RunExample.py
```

## Hints

See also the [hints](./Hints.md) (mainly for developer).
