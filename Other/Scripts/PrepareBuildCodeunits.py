import os
from pathlib import Path
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.ScriptCollectionCore import ScriptCollectionCore
from ScriptCollection.TasksForCommonProjectStructure import TasksForCommonProjectStructure


@GeneralUtilities.check_arguments
def update_iplocation_submodule(repository_folder: str):
    sc: ScriptCollectionCore = ScriptCollectionCore()
    submodule_folder = GeneralUtilities.resolve_relative_path("Other/Resources/Submodules/ip-location-db", repository_folder)
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


def prepare_build_codeunits():
    t = TasksForCommonProjectStructure()
    current_file = str(Path(__file__).absolute())
    repository_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    t.ensure_certificate_authority_for_development_purposes_is_generated(current_file)
    update_iplocation_submodule(repository_folder)


if __name__ == "__main__":
    prepare_build_codeunits()
