from ScriptCollection.TFCPS.DotNet.TFCPS_CodeUnitSpecific_DotNet import TFCPS_CodeUnitSpecific_DotNet_Functions,TFCPS_CodeUnitSpecific_DotNet_CLI
 
def build():

    platforms = ["win-x64", "linux-x64"]
    tf:TFCPS_CodeUnitSpecific_DotNet_Functions=TFCPS_CodeUnitSpecific_DotNet_CLI.parse(__file__)
    tf.build(platforms, True) 
    tf.tfcps_Tools_General.merge_sbom_file_from_dependent_codeunit_into_this(tf.get_codeunit_folder(),tf.get_codeunit_name(),"CountryInformationBackend",tf.use_cache())
    
if __name__ == "__main__":
    build()
