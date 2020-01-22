$(document).ready(function () {

    window.setTimeout(function () {
        $(".alert").fadeTo(1000, 0).slideUp(1000, function () {
            $(this).remove();
        });
    }, 60000);

    $("#LeagueId").change(function () {
        $("#TeamId").empty();
        $("#TeamId").append('<option value="0">[Seleccione un Equipo...]</option>');
        $.ajax({
            type: 'post',
            url: Url,
            dataType: 'json',
            data: { leagueId: $("#LeagueId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#TeamId").append('<option value="'
                        + data.TeamId + '">'
                        + data.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Falló al recuperar equipos.' + ex);
            }
        });
        return false;
    });

    $("#FavoriteLeagueId").change(function () {
        $("#FavoriteTeamId").empty();
        $("#FavoriteTeamId").append('<option value="0">[Seleccione un Equipo...]</option>');
        $.ajax({
            type: 'post',
            url: Url,
            dataType: 'json',
            data: { leagueId: $("#FavoriteLeagueId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#FavoriteTeamId").append('<option value="'
                        + data.TeamId + '">'
                        + data.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Falló al recuperar equipos.' + ex);
            }
        });
        return false;
    });

    $("#TournamentGroupId").change(function () {
        $("#LocalId").empty();
        $("#LocalId").append('<option value="0">[Seleccione un Equipo...]</option>');
        $("#VisitorId").empty();
        $("#VisitorId").append('<option value="0">[Seleccione un Equipo...]</option>');
        $.ajax({
            type: 'post',
            url: UrlMatch,
            dataType: 'json',
            data: { tournamentGroupId: $("#TournamentGroupId").val() },
            success: function (data) {
                $.each(data, function (i, data) {
                    $("#LocalId").append('<option value="'
                        + data.TeamId + '">'
                        + data.Name + '</option>');
                });

                $.each(data, function (i, data) {
                    $("#VisitorId").append('<option value="'
                        + data.TeamId + '">'
                        + data.Name + '</option>');
                });
            },
            error: function (ex) {
                alert('Falló al recuperar equipos.' + ex);
            }
        });
        return false;
    });


    $("#SelectedUser").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Generic/GetPlayers",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.FullDescription, value: item.UserId };
                    }));

                }
            });
        },
        minLength: 3,
        focus: function (event, ui) {
            $("#SelectedUser").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#SelectedUser").val(ui.item.label);
            $("#UserId").val(ui.item.value);
            return false;
        }

    })
    //.autocomplete("instance")._renderItem = function (ul, item) {
    //        return $("<li>")
    //            .append("<div>" + item.label + "<br>" + item.value + "</div>")
    //            .appendTo(ul);
    //    }
    ;
});

