from pathlib import Path
import re
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore
from ScriptCollection.TFCPS.TFCPS_Tools_General import TFCPS_Tools_General
from ScriptCollection.ImageUpdater import ImageUpdaterHelper, ConcreteImageUpdaterForDebian

@GeneralUtilities.check_arguments
def update_debian_version():
    concreteImageUpdaterForDebian=ConcreteImageUpdaterForDebian()
    latest_debian_tag=concreteImageUpdaterForDebian.version_to_tag(ImageUpdaterHelper.get_latest_version(concreteImageUpdaterForDebian.get_all_available_versions("debian")))

    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    debian_version_file: str = GeneralUtilities.resolve_relative_path("Other/Resources/Dependencies/Debian/Version.txt", repository_folder)
    GeneralUtilities.write_text_to_file(debian_version_file, latest_debian_tag)


@GeneralUtilities.check_arguments
def update_submodule_date_in_readme(repository_folder: str,sc:ScriptCollectionCore):
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    commitdate = sc.git_get_commit_date(submodule_folder)
    readme_file = GeneralUtilities.resolve_relative_path("./ReadMe.md", repository_folder)
    readme_content = GeneralUtilities.read_text_from_file(readme_file)
    date_regex = "The last update-date of the geo-ip-data is \\d\\d\\d\\d-\\d\\d-\\d\\d."
    GeneralUtilities.assert_condition(0 < len(re.findall(date_regex, readme_content, re.MULTILINE)), f"The readme does not contain a string matching the regex \"{date_regex}\".")
    readme_content = re.sub(date_regex, f"The last update-date of the geo-ip-data is {commitdate.strftime('%Y-%m-%d')}.", readme_content)
    GeneralUtilities.write_text_to_file(readme_file, readme_content)


def update_dependencies():
    current_file = str(Path(__file__).absolute())
    sc: ScriptCollectionCore = ScriptCollectionCore()
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    t: TFCPS_Tools_General = TFCPS_Tools_General(sc)
    t.update_submodule(repository_folder, "ip-location-db")
    update_submodule_date_in_readme(repository_folder,sc)
    update_debian_version()

if __name__ == "__main__":
    update_dependencies()
