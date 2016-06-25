Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Configurations

    Public Module ExtendedProperty

        <Extension>
        Public Function Ideogram(x As Circos) As Ideogram
            For Each include In x.Includes
                If TypeOf include Is Ideogram Then
                    Return DirectCast(include, Ideogram)
                End If
            Next

            Return Nothing
        End Function
    End Module
End Namespace