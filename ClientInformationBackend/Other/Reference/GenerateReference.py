import os
import sys
from pathlib import Path
import shutil
from ScriptCollection.GeneralUtilities import GeneralUtilities
from ScriptCollection.TasksForCommonProjectStructure import TasksForCommonProjectStructure


def update_api_spec_in_reference(current_file: str):
    codeunit_folder = GeneralUtilities.resolve_relative_path("../../..", current_file)
    api_spec_file_source: str = f"{codeunit_folder}/Other/Artifacts/APISpecification/ClientInformationBackend.latest.api.json"
    api_spec_file_target: str = f"{codeunit_folder}/Other/Reference/ReferenceContent/Attachments/ClientInformationBackend.latest.api.json"
    GeneralUtilities.assert_condition(os.path.isfile(api_spec_file_source), f"'{api_spec_file_source}' does not exist.")
    GeneralUtilities.ensure_file_does_not_exist(api_spec_file_target)
    shutil.copyfile(api_spec_file_source, api_spec_file_target)


def generate_reference():
    current_file = str(Path(__file__).absolute())
    TasksForCommonProjectStructure().standardized_tasks_generate_reference_by_docfx(current_file, 1,  "QualityCheck", sys.argv)
    update_api_spec_in_reference(current_file)


if __name__ == "__main__":
    generate_reference()
