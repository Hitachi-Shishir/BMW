<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" %>

<script runat="server">

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Notes Management</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet">
    <style>
        .tag-personal { color: #4169E1; }
        .tag-work { color: #FFA500; }
        .tag-social { color: #1E90FF; }
        .tag-important { color: #DC3545; }
        .note-card { transition: all 0.3s ease; }
        .note-card:hover { transform: translateY(-2px); box-shadow: 0 4px 8px rgba(0,0,0,0.1); }
        .avatar { width: 40px; height: 40px; border-radius: 50%; }
    </style>
</head>
<body>

<div class="container-fluid p-4">
    <div class="row">
        <!-- Sidebar -->
        <div class="col-md-3 mb-4">
            <div class="card">
                <div class="card-body">
                    <h5 class="mb-4"><i class="fas fa-sticky-note me-2"></i>Notes</h5>
                    
                    <h6 class="text-muted mb-3">Tags</h6>
                    <div class="d-flex flex-column gap-2">
                        <div class="d-flex align-items-center">
                            <span class="me-2 tag-personal">●</span>
                            <span>Personal</span>
                        </div>
                        <div class="d-flex align-items-center">
                            <span class="me-2 tag-work">●</span>
                            <span>Work</span>
                        </div>
                        <div class="d-flex align-items-center">
                            <span class="me-2 tag-social">●</span>
                            <span>Social</span>
                        </div>
                        <div class="d-flex align-items-center">
                            <span class="me-2 tag-important">●</span>
                            <span>Important</span>
                        </div>
                    </div>
                    
                    <button class="btn btn-grd-primary w-100 mt-4" data-bs-toggle="modal" data-bs-target="#addNoteModal">
                        <i class="fas fa-plus me-2"></i>Add New Note
                    </button>
                </div>
            </div>
        </div>

        <!-- Main Content -->
        <div class="col-md-9">
            <div class="row g-4" id="notesContainer">
                <!-- Notes will be dynamically added here -->
            </div>
        </div>
    </div>
</div>

<!-- Add Note Modal -->
<div class="modal fade" id="addNoteModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Add Note</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <form id="addNoteForm">
                    <div class="mb-3">
                        <label class="form-label">Title</label>
                        <input type="text" class="form-control" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">User Name</label>
                        <select class="form-select">
                            <option>Select User</option>
                            <option>John Doe</option>
                            <option>Max Smith</option>
                            <option>Kia Jain</option>
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Tag</label>
                        <select class="form-select">
                            <option>None</option>
                            <option>Personal</option>
                            <option>Work</option>
                            <option>Social</option>
                            <option>Important</option>
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Description</label>
                        <textarea class="form-control" rows="3" required></textarea>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-primary" onclick="addNote()">Add Note</button>
            </div>
        </div>
    </div>
</div>

<!-- Delete Confirmation Modal -->
<div class="modal fade" id="deleteModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Delete Notes</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body text-center">
                <i class="fas fa-trash-alt text-danger fs-1 mb-3"></i>
                <p>Are you sure you want to delete Notes?</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-danger" onclick="confirmDelete()">Delete</button>
            </div>
        </div>
    </div>
</div>

<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/js/bootstrap.bundle.min.js"></script>
<script>
    let notes = [
        {
            id: 1,
            title: 'Receive Package',
            user: 'John Doe',
            date: '11/02/2020',
            description: 'Facilisis curabitur facilisis vel elit sed dapibus sodales purus.',
            avatar: '/api/placeholder/40/40',
            tag: 'work'
        },
        {
            id: 2,
            title: 'Download Docs',
            user: 'Kia Jain',
            date: '11/04/2020',
            description: 'Proin a dui malesuada, laoreet mi vel, imperdiet diam quam laoreet.',
            avatar: '/api/placeholder/40/40',
            tag: 'personal'
        }
    ];

    function renderNotes() {
        const container = document.getElementById('notesContainer');
        container.innerHTML = notes.map(note => `
        <div class="col-6 mb-3">
            <div class="card note-card">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-start mb-3">
                        <div class="d-flex align-items-center">
                          
                            <div>
                                <h6 class="mb-0">${note.user}</h6>
                                <small class="text-muted">${note.date}</small>
                            </div>
                        </div>
                        <div class="dropdown">
                            <button class="btn btn-link" data-bs-toggle="dropdown">
                                <i class="fas fa-ellipsis-v"></i>
                            </button>
                            <ul class="dropdown-menu">
                                <li><a class="dropdown-item" href="#"><i class="fas fa-edit me-2"></i>Edit</a></li>
                                <li><a class="dropdown-item" href="#" onclick="showDeleteModal(${note.id})">
                                    <i class="fas fa-trash-alt me-2"></i>Delete</a></li>
                                <li><a class="dropdown-item" href="#"><i class="fas fa-eye me-2"></i>View</a></li>
                            </ul>
                        </div>
                    </div>
                    <h5 class="card-title">${note.title}</h5>
                    <p class="card-text">${note.description}</p>
                    <div class="d-flex justify-content-between align-items-center">
                        <span class="tag-${note.tag}">●</span>
                        <div>
                            <button class="btn btn-link text-danger"><i class="fas fa-trash-alt"></i></button>
                            <button class="btn btn-link text-warning"><i class="fas fa-star"></i></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `).join('');
    }

    function addNote() {
        const form = document.getElementById('addNoteForm');
        const formData = new FormData(form);
        // Add note logic here
        const modal = bootstrap.Modal.getInstance(document.getElementById('addNoteModal'));
        modal.hide();
        renderNotes();
    }

    function showDeleteModal(noteId) {
        const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
        deleteModal.show();
    }

    function confirmDelete() {
        // Delete note logic here
        const modal = bootstrap.Modal.getInstance(document.getElementById('deleteModal'));
        modal.hide();
        renderNotes();
    }

    // Initial render
    renderNotes();
</script>

</body>
</html>
</asp:Content>

