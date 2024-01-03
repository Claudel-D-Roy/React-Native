import React from 'react';

function Pagination({ currentPage, totalPages, setCurrentPage, prevPage, nextPage }) {
    const pageNumbers = [];

    if (totalPages <= 1) {
        return null;
    }

    pageNumbers.push(1);

    let startPage = Math.max(2, currentPage - 2);
    let endPage = Math.min(totalPages - 1, currentPage + 2);

    if (currentPage <= 4) {
        startPage = 2;
        endPage = Math.min(5, totalPages - 1);
    }

    if (totalPages < 5) {
        endPage = totalPages - 1;
    }

    const showStartEllipsis = startPage > 2;
    const showEndEllipsis = endPage < totalPages - 1;

    if (showStartEllipsis) {
        pageNumbers.push('...');
    }

    for (let i = startPage; i <= endPage; i++) {
        pageNumbers.push(i);
    }

    if (showEndEllipsis) {
        pageNumbers.push('...');
    }

    if (totalPages > 1 && !pageNumbers.includes(totalPages)) {
        pageNumbers.push(totalPages);
    }

    const renderPageNumbers = pageNumbers.map((pageNumber, index) => {
        if (pageNumber === '...') {
            return <span key={`ellipsis-${index}`}>...</span>;
        } else {
            const handleClick = pageNumber !== '...' ? () => setCurrentPage(pageNumber) : undefined;
            return (
                <span
                    key={pageNumber}
                    className={`page-number ${pageNumber === currentPage ? 'current-page' : ''}`}
                    onClick={handleClick}
                >
                    {pageNumber}
                </span>
            );
        }
    });

    return (
        <div className="pagination">
            <button
                className="page-button"
                onClick={prevPage}
                disabled={currentPage === 1}
            >
                Previous
            </button>
            {renderPageNumbers}
            <button
                className="page-button"
                onClick={nextPage}
                disabled={currentPage >= totalPages}
            >
                Next
            </button>
        </div>
    );
}

export default Pagination;