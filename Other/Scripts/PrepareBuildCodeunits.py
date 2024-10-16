from pathlib import Path
import re
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore
from ScriptCollection.TasksForCommonProjectStructure import TasksForCommonProjectStructure


def update_submodule_date_in_readme(repository_folder):
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    sc: ScriptCollectionCore = ScriptCollectionCore()
    commitdate = sc.git_get_commit_date(submodule_folder)
    readme_file = GeneralUtilities.resolve_relative_path("./ReadMe.md", repository_folder)
    readme_content = GeneralUtilities.read_text_from_file(readme_file)
    date_regex = "The last update-date of the geo-ip-data is \\d\\d\\d\\d-\\d\\d-\\d\\d."
    GeneralUtilities.assert_condition(0 < len(re.findall(date_regex, readme_content, re.MULTILINE)), f"The readme does not contain a string matching the regex \"{date_regex}\".")
    readme_content = re.sub(date_regex, f"The last update-date of the geo-ip-data is {commitdate.strftime('%Y-%m-%d')}.", readme_content)
    GeneralUtilities.write_text_to_file(readme_file, readme_content)


def update_iplocation_submodule(repository_folder):
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    sc: ScriptCollectionCore = ScriptCollectionCore()
    sc.git_fetch(submodule_folder, "origin")
    sc.git_checkout(submodule_folder, "main")
    sc.git_pull(submodule_folder, "origin", "main", "main")


def prepare_build_codeunits():
    t = TasksForCommonProjectStructure()
    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    t.ensure_certificate_authority_for_development_purposes_is_generated(current_file)
    update_iplocation_submodule(repository_folder)
    update_submodule_date_in_readme(repository_folder)


if __name__ == "__main__":
    prepare_build_codeunits()
