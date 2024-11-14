import sys
import os
import shutil
from pathlib import Path
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.TasksForCommonProjectStructure import TasksForCommonProjectStructure


@GeneralUtilities.check_arguments
def update_data_from_submodule(codeunit_folder: str) -> None:
    codeunit_name: str = os.path.basename(codeunit_folder)
    repository_folder = GeneralUtilities.resolve_relative_path("..", codeunit_folder)
    datasubmodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    geo_information_file = GeneralUtilities.resolve_relative_path("geolite2-geo-whois-asn-country/geolite2-geo-whois-asn-country-ipv4.csv", datasubmodule_folder)
    target_folder = GeneralUtilities.resolve_relative_path(f"{codeunit_name}/Data/GeoIPData.csv", codeunit_folder)
    shutil.copyfile(geo_information_file, target_folder)
    update_data_from_submodule(repository_folder)


def update_dependencies():
    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    TasksForCommonProjectStructure().update_dependencies_of_typical_dotnet_codeunit(current_file, 1, sys.argv)
    update_data_from_submodule(repository_folder)


if __name__ == "__main__":
    update_dependencies()
