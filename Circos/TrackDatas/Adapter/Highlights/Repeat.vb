﻿#Region "Microsoft.VisualBasic::e7960fcc4cc15725cc863a98473d765c, ..\interops\visualize\Circos\Circos\TrackDatas\Adapter\Highlights\Repeat.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports SMRUCC.genomics.ComponentModel
Imports SMRUCC.genomics.SequenceModel.FASTA
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq.Extensions

Namespace TrackDatas.Highlights

    Public Class Repeat : Inherits Highlights

        Sub New(repeat As IEnumerable(Of NtProps.Repeat), attrs As IEnumerable(Of Double))
            Dim clMaps As IdentityColors = New IdentityGradients(attrs.Min, attrs.Max, 512)
            Dim v As Double() = attrs.ToArray
            Me.__source = New List(Of ValueTrackData)(repeat.ToArray(Function(x) __creates(x, maps:=clMaps, attrs:=v)))
        End Sub

        Private Shared Function __creates(loci As NtProps.Repeat, maps As IdentityColors, attrs As Double()) As ValueTrackData
            Dim left As Integer = CInt(Val(loci.Minimum.Replace(",", "")))
            Dim Right As Integer = CInt(Val(loci.Maximum.Replace(",", "")))
            Dim r As Double() = attrs.Skip(left).Take(Right - left).ToArray

            Return New ValueTrackData With {
                .start = left,
                .end = Right,
                .formatting = New Formatting With {
                    .fill_color = maps.GetColor(r.Average)
                }
            }
        End Function

        Sub New(repeat As IEnumerable(Of NtProps.Repeat), Optional Color As String = "Brown")
            Me.__source =
                LinqAPI.MakeList(Of ValueTrackData) <= From x As NtProps.Repeat
                                                       In repeat
                                                       Select New ValueTrackData With {
                                                           .start = CInt(Val(x.Minimum)),
                                                           .end = CInt(Val(x.Maximum)),
                                                           .formatting = New Formatting With {
                                                               .fill_color = Color
                                                           }
                                                       }
        End Sub
    End Class
End Namespace
