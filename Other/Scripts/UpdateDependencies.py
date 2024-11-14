import os
from pathlib import Path
import re
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore


@GeneralUtilities.check_arguments
def update_submodule_date_in_readme(repository_folder: str):
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    sc: ScriptCollectionCore = ScriptCollectionCore()
    commitdate = sc.git_get_commit_date(submodule_folder)
    readme_file = GeneralUtilities.resolve_relative_path("./ReadMe.md", repository_folder)
    readme_content = GeneralUtilities.read_text_from_file(readme_file)
    date_regex = "The last update-date of the geo-ip-data is \\d\\d\\d\\d-\\d\\d-\\d\\d."
    GeneralUtilities.assert_condition(0 < len(re.findall(date_regex, readme_content, re.MULTILINE)), f"The readme does not contain a string matching the regex \"{date_regex}\".")
    readme_content = re.sub(date_regex, f"The last update-date of the geo-ip-data is {commitdate.strftime('%Y-%m-%d')}.", readme_content)
    GeneralUtilities.write_text_to_file(readme_file, readme_content)


@GeneralUtilities.check_arguments
def update_iplocation_submodule(repository_folder: str):
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
    sc: ScriptCollectionCore = ScriptCollectionCore()
    sc.git_fetch(submodule_folder, "origin")
    sc.git_checkout(submodule_folder, "main")
    sc.git_pull(submodule_folder, "origin", "main", "main", True)
    current_version = sc.get_semver_version_from_gitversion(repository_folder)
    changelog_file = os.path.join(repository_folder, "Other", "Resources", "Changelog", f"v{current_version}.md")
    if (not os.path.isfile(changelog_file)):
        GeneralUtilities.ensure_file_exists(changelog_file)
        GeneralUtilities.write_text_to_file(changelog_file, """# Release notes

## Changes

- Updated geo-ip-database.
""")


def update_dependencies():
    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    update_iplocation_submodule(repository_folder)
    update_submodule_date_in_readme(repository_folder)


if __name__ == "__main__":
    update_dependencies()
