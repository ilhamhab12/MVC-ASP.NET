function deleteItem(Id)
{
    debugger;
    swal({
        title: "Are you sure?",
        text: "Are you sure that you want to delete this Order?",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonText: "Yes, delete it!",
        confirmButtonColor: "#ec6c62"
    },
        function()
        {
            $.ajax({
                url: "http://localhost:1873/Items/Delete/" + Id,
                data:
                {
                    "Id": Id
                },
                type: "POST",
                //type: "DELETE",
                //done di gantikan dengan success
                success: function (response) {
                    swal({
                        title: "Deleted!",
                        text: "Your item was deleted",
                        type: "success"
                    },
                        function (){
                            window.location.href = '/Items/Index/';
                        });
                },
                error: function (response) {
                    swal("Oops!", "We couldn't connect to the server", "error")
                }
            });
        });
}
                    //.done(function(data)
                    //{
                    //    sweetAlert
                    //        ({
                    //            title: "Deleted!",
                    //            text: "Your file was successfully deleted!",
                    //            type: "success"
                    //        },
                    //            function()
                    //            {
                    //                window.location.href = '/ItemsController/Index';
                    //            });
                    //})
                    //.error(function(data)
                    //{
                    //    swal("Oops", "We couldn't connect to the server!", "error");
                    
                    //});
                //});
            //}



