<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManobraCadastro.aspx.cs" Inherits="EstaparCRUD.Views.Manobras.ManobraCadastro" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(window).on('load', function (event) {
            $('#preloader1').delay(0).fadeOut('slow', function () {
            });
        });
        $(function () {

            $(".btn-preloader").on('click', function (event) {
                $('#preloader1').delay(0).fadeIn('fast', function () {
                });
                $("#genCortina").fadeIn('fast');
            });
        });

    </script>
    <div class="card card-body">
        <div class="form-group row"></div>
        <div class="card card-default">
            <div class="card-header">
                <div class="row">
                    <div class="col-12 text-center">
                        CADASTRO DE MANOBRAS
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <span class="col-2">CARRO:</span>
                    <div class="col-4">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCarro" OnInit="ddlCarro_Init" >
                        </asp:DropDownList>
                    </div>
                    <span class="col-2">MANOBRISTA:</span>
                    <div class="col-4">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlManobrista" OnInit="ddlManobrista_Init">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-2">CLASSIFICAÇÃO:</span>
                    <div class="col-3">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlClassificacao" >
                            <asp:ListItem Text="Recepção do Veículo" Value="1" Selected="True"></asp:ListItem> 
                            <asp:ListItem Text="Retorno do veículo ao Cliente" Value="0"></asp:ListItem> 
                        </asp:DropDownList>
                    </div>
                    </div>
                <div class="form-group row">
                    <div class="col text-center">
                        <asp:LinkButton ID="btnCadastrar" runat="server" OnClick="btnCadastrar_Click" CssClass="btn btn-success btn-preloader">
                            <i class="fas fa-save"></i> CADASTRAR
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divCortina" runat="server" visible="false"></div>
</asp:Content>
