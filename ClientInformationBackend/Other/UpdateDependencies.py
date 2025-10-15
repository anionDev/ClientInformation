import sys
import os
import shutil
from pathlib import Path
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.TFCPS.DotNet.TFCPS_CodeUnitSpecific_DotNet import TFCPS_CodeUnitSpecific_DotNet_Functions,TFCPS_CodeUnitSpecific_DotNet_CLI


@GeneralUtilities.check_arguments
def update_data_from_submodule(codeunit_folder: str) -> None:
    codeunit_name: str = os.path.basename(codeunit_folder)
    repository_folder = GeneralUtilities.resolve_relative_path("..", codeunit_folder)
    datasubmodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    geo_information_file = GeneralUtilities.resolve_relative_path("geolite2-geo-whois-asn-country/geolite2-geo-whois-asn-country-ipv4.csv", datasubmodule_folder)
    target_folder = GeneralUtilities.resolve_relative_path(f"{codeunit_name}/Data/GeoIPData.csv", codeunit_folder)
    shutil.copyfile(geo_information_file, target_folder)


def update_dependencies():
    current_file = str(Path(__file__).absolute())
    codeunit_folder = GeneralUtilities.resolve_relative_path("../..", current_file)
    tf:TFCPS_CodeUnitSpecific_DotNet_Functions=TFCPS_CodeUnitSpecific_DotNet_CLI.parse(__file__)
    tf.update_dependencies()
    update_data_from_submodule(codeunit_folder)


if __name__ == "__main__":
    update_dependencies()
